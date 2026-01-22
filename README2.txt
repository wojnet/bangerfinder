==================================================
BANGERFINDER - PROJECT TEST INSTRUCTIONS
==================================================
This project is fully containerized. The database 
automatically migrates on startup.

HOW TO RUN:
1. Open a terminal in this root folder.
2. Run: docker-compose up --build

ACCESS POINTS:
- Frontend (Next.js/Nginx): http://localhost:3000

TROUBLESHOOTING NOTE:
You may see "Cross-Origin Request Blocked" errors in the browser 
console for 'http://localhost/login/'. These are background 
prefetch requests from Next.js and do not affect functionality. 
Actual Register/Login API are fully functional.

