SELECT *
FROM Cars
CROSS JOIN ( SELECT * FROM CarLockedStatus ORDER BY LockedTimeStamp DESC LIMIT 1);
