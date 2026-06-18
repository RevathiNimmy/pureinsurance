SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
 
EXECUTE DDLDropProcedure 'spu_check_writeoff'
GO

CREATE PROCEDURE spu_check_writeoff
    @user_Id smallint,
    @document_id int,
    @amount numeric(19,4),
    @invalid_write_off tinyint output 
AS
BEGIN
if exists(select d.document_ref 
	  from document d
	  join documenttype dt on d.documenttype_id = dt.documenttype_id
	  where dt.description = 'Write Off'
          and d.document_id = @document_id)
and
	 isnull((select u.Write_off_Amount from user_authorities u 
         where u.user_id =  @user_ID 
 	 and has_write_off_authority = 1),0) < abs(@amount)
begin
	select @invalid_write_off = 1
end
else
	select @invalid_write_off = 0
 

 
END