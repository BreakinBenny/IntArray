# Money Tracker Project
This application is a database-like bank system that keeps track of transactions, and can save and load a history of them from a .CSV or .TXT file.

* It starts by (if you submitted a valid extension) trying to load a file named transactionHistory.csv, but can also load TXT and JSON.
* It gives you some money to start if you choose not to set a starting balance, as long as you provide more than zero.
* Expense can't be greater than what you have, nor can it (let alone the income) be zero or less.
* File must be named specifically **transactionHistory.(fileExtension)** to not start fresh, and it's exported to the application's directory but is saved in the repository's one.

Upon saving and quitting, you must specify whether it's a JSON, CSV or TXT. Non-JSON was my priority.
