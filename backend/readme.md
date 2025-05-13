# Compulsory Assignment 2
### Daniels, Ivan and Ane
## Second-Hand E-Commerce Backend Design

This repository contains the design documentation and backend structure implementation for a second-hand e-commerce platform, as part of the "Databases for Developers" course.

## Database Selection

### Strategy:
We have opted for a hybrid database approach, utilizing both NoSQL and Relational databases. Having these two different database models has multiple benefits like faster retrieval times for the NoSQL, when scaling the program. The NoSQL database is also known as the schemaless database. It has faster look up times, but compared to the relational database, it has slow writing times.
The benefit of using the relational database is the faster writing to the database, and the benefit of using the NoSQL database is the faster look up times. The solution to get the benefits from both of the databases, is to use both of them at the same time, but for different purposes.
The ACID-principles has been kept in mind, when separating the Read and Write commands in the application to use the access patterns within the application. The access pattern used in the application is the CQRS-pattern. This approach allows us to leverage the specific strengths of each database paradigm.

The databases contain the same data and the data in the databases are synchronised. This causes the data to be redundant, in the sense that it has been duplicated, because the same data is present in both databases. The reason behind having 2 databases containing the same data is to exploit the benefits from both databases, regarding the retrieval and writing times. In the economic sense, it is commonly cheaper to scale out instead of up. This means it is cheaper to have 2 different types of databases and use them simultaneously, than to upgrade for example the hardware used for one of them.

### NoSQL Database: MongoDB

#### Data Managed:
All data

#### Justification:
- **Flexible Schema:** Second-hand listings have highly variable attributes (e.g., clothing size/material vs. electronic specs). MongoDB's document model easily accommodates this diversity without enforcing a rigid schema across all item types. User profiles and reviews also benefit from this flexibility.
- **Scalability:** Listings and user-generated reviews can grow significantly. MongoDB offers strong horizontal scalability options (sharding) suitable for handling large volumes of data and potentially high read/write loads associated with browsing and creating listings/reviews.
- **Development Speed:** Mapping application objects to JSON-like documents can streamline development for these types of data.

### Relational Database: MSSQL

#### Data Managed:
All data

#### Justification:
- **Data Integrity & Structure:** Order data is inherently structured and requires high consistency. MSSQL enforces schema and relational constraints effectively.
- **ACID Transactions:** Placing an order is a critical, multi-step process (e.g., create order record, update listing status). MSSQL provides robust, mature support for ACID transactions across multiple tables, ensuring these operations complete atomically and reliably, which is crucial for maintaining data consistency in financial/fulfillment contexts.
- **Complex Queries:** Relational databases excel at complex queries, joins, and aggregations often needed for order reporting and analysis.

### Why not only use one database type?
While MongoDB supports multi-document transactions, leveraging MSSQL for orders provides industry-standard, robust transactional guarantees more naturally for this highly structured and critical data.
These features from both databases are preferred, and one of the reasons for using both of them.
It is more economically efficient to have 2 instances of data-base, regarding the scaling of the system compared to scaling on the hardware.

## Synchronization of the databases
The synchronisation of the databases are handled in the Infrastructure layer in the application. This could have been done on the database-level, by using triggers in the database. This approach is not going to be implemented, since it is harder to trace and log what is happening in the backend, when debugging.
Instead of creating triggers in the database, the synchronization of the database data is implemented in the Infrastructure layer. The synchronisation happens, when the data in the database is modified. This starts a transaction, where the data is updated in the relational database and later updated in the NoSQL database. This is achieved by implementing the ‘unit of work’ - pattern for updating the data in the database, with an eventbus.

## Integration of cloud storage

### Technology:
AWS S3 (It’s an industry standard, scalable, reliable object storage, supported by a massive company).

### Purpose:
Storing user-uploaded item images (and potential other media in the future). Databases are not suitable for storing large binary files, such as images, thus AWS S3.

### Interaction Flow for ‘an image upload for a listing’
- User uploads an image via the backend API (e.g., POST /listings/{id}/images) (Can be done after listing with the textual data has been created).
- The backend service receives the file.
- The backend service uploads the image file to an S3 bucket.
- S3 returns a unique, publicly accessible URL for the stored image.
- The backend service saves this URL string, associating it with the listing in the database,
- When a client requests the listing details, the backend provides the listing data including the associated images.
- The client's browser/app then fetches and displays the images directly from the S3 URLs.

### Database Interaction:
The database only stores the reference (URL) to the media file, not the file itself.

## Caching Strategy

### Technology:
Redis (In-memory data store known for speed and versatility).

### Purpose:
Improve read performance for frequently accessed, relatively static data, reducing load on primary databases.

### Data to Cache:
- Individual Listing details (GET /listings/{id}).
- Results for common browse/search queries (GET /listings?category=..., potentially paginated).
- User Profiles (GET /users/{id}).

### Cache Invalidation Strategy:
- **Time-To-Live (TTL):** Primarily using TTL for simplicity given the project constraints. Cached items will expire after a defined period (e.g., 5 minutes). This provides a balance between performance and data freshness.
- **Potential Write-Through (Conceptual):** For a more robust system, cache entries (e.g., for a specific listing) would be explicitly invalidated/updated when the underlying data changes (e.g., when a listing is updated or sold). We acknowledge this but will rely on TTL for this implementation scope.

### Implementation:
A CacheService abstraction will be used. Services like ListingService will first attempt to fetch data from the cache; on a cache miss, they will fetch from the database, store the result in the cache with a TTL, and then return the data.

## Transaction management
For operations like placing orders or updating listings, ensuring data consistency is critical. Our approach to transactional integrity primarily revolves around the Unit of Work pattern for database operations. Operations involving external services like cloud storage have a compensating action. This is outside of the scope of the unit of work. Commands (as per CQRS) are responsible for state changes and are therefore the focus point for transaction management.

To be more technical, when placing an order, the operations such as inserting an order record in DB and updating listing’s status (i.e. sold) - must be atomic.
Inserts, updates, and deletes are grouped into a single atomic transaction. This is executed on IUnitOfWork’s SaveChangesAsync() (which wraps database context’s SaveChangesAsync()). It attempts to commit all of the tracked changes. If any operation fails or an exception occurs, the transaction is rolled back (thus ensuring atomicity for the database part).

Looking at the scenario of listing creation with image uploads, we can see that an external service is involved. Our approach to handling it is the following:
First, attempt to upload the images for the listing. If an image upload fails, the handler will stop processingm deleting any previous images already uploaded for that request (a compensating action), No db transaction is initiated. The user is notified of failure and can retry.
If the necessary cloud operations (image uploads) are successful, the command handler creates the listing in db. This is where all db changes are atomically committed using units of work.