DROP TABLE IF EXISTS OrganizationsProfit;
DROP TABLE IF EXISTS OrganizationsSecurity;
DROP TABLE IF EXISTS Report2;

CREATE TEMP TABLE IF NOT EXISTS OrganizationsProfit AS
select * from (
	select o.organizationid, sum(price) from (
		select * from organizations o
		where o.type = 'Commercial'
	) o
	join contracts c on c.organizationid = o.organizationid
	where c.isLegalEntity = 'true'
	group by o.organizationid
) g
order by g.sum desc;

CREATE TEMP TABLE IF NOT EXISTS OrganizationsSecurity AS
SELECT g.organizationid, COUNT(a.alarmid) FROM (
	SELECT o.organizationid, c.contractid FROM (
		SELECT o.organizationid FROM organizations o
		WHERE o.type = 'Commercial'
	) o
	JOIN contracts c ON c.organizationid = o.organizationid
	WHERE c.isLegalEntity = 'true'
) g
JOIN alarms a ON a.contractid = g.contractid
GROUP BY g.organizationid
ORDER BY COUNT(a.alarmid);

CREATE TEMP TABLE IF NOT EXISTS Report2 AS
SELECT o.name, sum, count FROM OrganizationsProfit op
JOIN OrganizationsSecurity os ON op.organizationid = os.organizationid
JOIN organizations o on o.organizationid = op.organizationid
ORDER BY sum desc, count
LIMIT 5;

SELECT * FROM Report2;