set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'party_account_handler'
go

CREATE VIEW dbo.party_account_handler AS
SELECT ph.party_cnt, ph.forename, ph.initials, ph.department_id, ph.party_title_code ,ph.UserId,ph.UniqueId,ph.ScreenHierarchy 
FROM party_handler ph  
INNER JOIN party p  
ON ph.party_cnt = p.party_cnt  
INNER JOIN party_type pt  
ON pt.party_type_id = p.party_type_id  
WHERE (pt.code = 'AH')

go