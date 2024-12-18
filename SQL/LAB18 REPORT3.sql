select c.name, cc.calltype, cc.contractid, firstname, lastname, middlename, phonenumber from crews c
join crewmembers cm on cm.memberid = c.leaderid
join users u on cm.userid = u.userid
join crewcalls cc on cc.crewid = c.crewid