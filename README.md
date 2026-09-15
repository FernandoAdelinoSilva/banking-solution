# Banking API

## 📌 Overview
Banking API is a simple service that manages bank accounts with operations for **deposit**, **withdraw**, **transfer**, and **balance inquiry**.  
The business logic is **real and consistent**: accounts are created, balances are updated, and errors are thrown when rules are violated.

---

## 🚀 Endpoints

POST /Account/reset
Clears all accounts and balances.

### 📊 Get account balance
GET /Account/balance?accountId={id}
- Returns the balance of the given account.
- If the account does not exist → `404 0`.

### 💰 Post account event
POST /Account/event
Content-Type: application/json

{
"type": "deposit | withdraw | transfer",
"origin": "string",
"destination": "string",
"amount": decimal
}

## 📌 Business Rules
- **Deposit**: Creates the account if it does not exist, or increases the balance if it does.  
- **Withdraw**: Requires the origin account to exist and have sufficient funds.  
- **Transfer**: Requires the origin account to exist; destination is created automatically if it does not exist.  
- **Insufficient funds**: Throws an error (`InvalidOperationException`).  
- **Non-existing account**: Operations fail with error (`404 0`).  

---

## ✅ Test Coverage
Unit tests cover:
- Non-existing account → error.  
- Initial deposit → creates account.  
- Subsequent deposit → increases balance.  
- Valid withdraw → decreases balance.  
- Withdraw from non-existing account → error.  
- Valid transfer → moves funds and creates destination.  
- Transfer from non-existing origin → error.  
- Insufficient funds → error.

---

## ▶️ How to Run
1. Clone the repository

2. Navigate into the project:
cd banking-api

3. Run tests:
dotnet test

4. Start the API:
dotnet run

## ▶️ Example Requests
Deposit:
curl -X POST http://localhost:5000/Account/event \
  -H "Content-Type: application/json" \
  -d '{"type":"deposit","destination":"100","amount":10}'

Withdraw:
curl -X POST http://localhost:5000/Account/event \
  -H "Content-Type: application/json" \
  -d '{"type":"withdraw","origin":"100","amount":5}'

Transfer: 
curl -X POST http://localhost:5000/Account/event \
  -H "Content-Type: application/json" \
  -d '{"type":"transfer","origin":"100","destination":"300","amount":15}'

Balance
curl "http://localhost:5000/Account/balance?accountId=100"

## 📌 Notes
- The API is designed to be stateless between resets.
- _accountStore maintains accounts in memory; persistence can be added later.
- Thread-safety should be considered if running in a multi-threaded environment.

## 📌 Future Improvements
- Database persistence: Replace in-memory store with a relational or NoSQL database.
- Authentication & authorization: Secure endpoints with user identity and permissions.