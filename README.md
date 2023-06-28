Stripe Balance API Application
This application provides a set of endpoints that interact with the Stripe API to retrieve balance information. It is implemented in .NET using a RESTful approach and is designed to be consumed by other services or applications.

Description
The application is split into two main controllers:

BalancesController
BalanceTransactionsController
Each controller serves a specific purpose within the application and interacts with the Stripe API using a defined StripeAccessFacade.

BalancesController
This controller provides an endpoint to get balance information.

GET api/Balances endpoint: Returns the list of balances from Stripe. If no balances are found, a 404 (Not Found) status code is returned. Any errors encountered during the process are logged and appropriately returned to the client.
BalanceTransactionsController
This controller provides an endpoint to get balance transactions information.

GET api/BalanceTransactions endpoint: Returns a paginated list of balance transactions from Stripe. Pagination parameters can be provided in the request. If the pagination parameters are not valid, a 400 (Bad Request) status code is returned. If both StartAfter and EndBefore pagination options are provided, a 400 (Bad Request) status code is returned with an error message since both cannot be specified at the same time. Any errors encountered during the process are logged and appropriately returned to the client.

How to use locally:
Run StripeTestAPI profile.

Examples of URLs how to use developed endpoint:
https://localhost:5001/swagger/index.html

https://localhost:5001/api/Balances - get

https://localhost:5001/api/BalanceTransactions?PageSize=3 - get

https://localhost:5001/api/BalanceTransactions?PageSize=3&StartAfter=txn_3NNu512eZvKYlo2C0yJILUuo - get

https://localhost:5001/api/BalanceTransactions?PageSize=3&EndBefore=txn_3NNu4G2eZvKYlo2C0Zxwkiqr - get