select c.crewid, c.name, leaderid, firstname, lastname, middlename, phonenumber from crews c
join crewmembers cm on cm.memberid = c.leaderid
join users u on cm.userid = u.userid