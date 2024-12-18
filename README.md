# LibraryAPI
RESTful API Development
- Project Setup
    i. Use .NET 8/9 to create a Web API project.
    ii. Configure Entity Framework Core with a SQLite or SQL Server
    database.
- Entity Framework
    i. Define a Book entity with the following properties: Id, Title, Author,
    ISBN, PublishedDate.
    ii. Set up a LibraryContext for Entity Framework.
    iii. Implement CRUD operations for the Book entity.
- Endpoints
    i. GET /api/books: Retrieve all books.
    ii. GET /api/books/{id}: Retrieve a book by ID.
    iii. POST /api/books: Add a new book.
    iv. PUT /api/books/{id}: Update an existing book.
    v. DELETE /api/books/{id}: Delete a book by ID.
- Security
    i. Implement JWT authentication.
    ii. Secure all CRUD endpoints for authenticated users only.
    iii. Add endpoints for user registration and login to generate JWT
    tokens.
    iv. Validation and Error Handling
    v. Validate input data such as ensuring Title and Author fields are
    non-empty.
    vi. Implement appropriate error handling with meaningful HTTP status
    codes.
- Documentation
    i. Swagger for API documentation
