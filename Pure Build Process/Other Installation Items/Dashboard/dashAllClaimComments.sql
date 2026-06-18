DDLDropview 'dashAllClaimComments'
go
Create view dashAllClaimComments as 
select claim_id, 
       comment_type,
       claim_comment_id,
       replace(replace(replace(comment_line, CHAR(9), ''), CHAR(10), ' '), CHAR(13), '') as comment_line
from 
	claim_comments c
where 
	len(comment_line)>0 