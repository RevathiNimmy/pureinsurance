SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_SIR_Cover_Note_Book_Find'
GO

CREATE  PROCEDURE spu_SIR_Cover_Note_Book_Find
	@book_number varchar(50) = null,
	@start_number int = null,
	@end_number int = null,
	@agent_cnt int = null,
	@last_updated datetime = null,
	@source_id int = null,
	@cover_note_book_status_id int = null,
	@policy_number varchar(50) = null,
	@assigned_date datetime = null,
	@user_id int = null,
        @MaxRowsToFetch INT = -1
AS
BEGIN
	DECLARE @sql varchar(1000),
		@sSQL varchar(3000),
		@sWhere varchar(8)

	Set @sWHERE = ' WHERE '
	Set @sql = ''
	Set @sSQL = ''
	IF @MaxRowsToFetch<>-1
	BEGIN
        SET @sSQL = 'SET NOCOUNT ON' + CHAR(13) + CHAR(10)   
	SET @sSQL = @sSQL +'SET ROWCOUNT '
	SET @sSQL = @sSQL + CONVERT(VARCHAR(5),@MaxRowsToFetch) + CHAR(13) + CHAR(10)
	END 	
	Set @sSQL = @sSQL + 'SELECT DISTINCT CNB.cover_note_book_id, CNB.book_number, CNB.start_number, CNB.end_number, CNB.Agent_Cnt, '
	Set @sSQL = @sSQL + 'P.Name, CNB.cover_note_book_status_id, CNBS.description, '
	Set @sSQL = @sSQL + 'CNB.Source_Id, S.description, cnb.last_updated, cnb.created_date, cnb.effective_date '
	Set @sSQL = @sSQL + 'FROM Cover_Note_Book CNB'
	Set @sSQL = @sSQL + ' INNER JOIN Cover_Note_Book_status CNBS ON CNB.cover_note_book_status_id = CNBS.cover_note_book_status_id AND CNB.cover_note_book_status_id = ISNULL(' + isnull(cast(@cover_note_book_status_id as varchar), 'NULL') + ', CNB.cover_note_book_status_id)'
	Set @sSQL = @sSQL + ' LEFT JOIN Party P ON P.party_cnt = CNB.agent_cnt'
	Set @sSQL = @sSQL + ' LEFT JOIN Source S ON S.source_id = CNB.source_id'

	IF (@policy_number <> NULL)
	Begin
	    Set @sSQL = @sSQL + ' INNER JOIN Cover_Note_Sheet CNS ON CNS.cover_note_book_id = CNB.cover_note_book_id'
	    Set @sSQL = @sSQL + ' INNER JOIN Insurance_File IFI ON IFI.insurance_file_cnt = CNS.insurance_file_cnt AND ifi.insurance_ref like ' + "'" + @policy_number + "'"
	End

	IF (@book_number <> NULL)
	BEGIN
	    Set @sql = @sWhere + 'CNB.book_number LIKE ' + "'" + @book_number + "'"
	    Set @sWhere = ' AND '
	END

	IF (@start_number <> NULL)
	BEGIN
	    Set @sql = @sql + @sWhere + 'CNB.start_number = ' + cast(@start_number as varchar)
	    Set @sWhere = ' AND '
	END

	IF (@end_number <> NULL)
	BEGIN
	    Set @sql = @sql + @sWhere + 'CNB.end_number = ' + cast(@end_number as varchar)
	    Set @sWhere = ' AND '
	END

	IF (@agent_cnt <> NULL)
	BEGIN
	    Set @sql = @sql + @sWhere + 'CNB.agent_cnt = ' + cast(@agent_cnt as varchar)
	    Set @sWhere = ' AND '
	END

	IF (ISNULL(@last_updated, '') <> '')
	BEGIN
	    Set @sql = @sql + @sWhere + 'DATEDIFF(day, CNB.last_updated, ' + "'" + cast(@last_updated as varchar) + "') = 0"
	    Set @sWhere = ' AND '
	END

	IF (@source_id <> NULL)
	BEGIN
	    Set @sql = @sql + @sWhere + 'CNB.source_id = ' + cast(@source_id as varchar)
	    Set @sWhere = ' AND '
	END

	 IF (@assigned_date <> NULL)  
	 BEGIN  
	     Set @sql = @sql + @sWhere + 'CNB.source_id > 0 AND DATEDIFF(day, CNB.last_updated, ' + "'" + cast(@assigned_date as varchar) + "') = 0"  
	     Set @sWhere = ' AND ' 
	 END
	IF (ISNULL(@user_id, 0 ) <> 0)
	    Set @sql = @sql + @sWhere + 's.source_id NOT IN (SELECT source_id FROM pmuser_source WHERE [user_id] =' + cast(@user_id as varchar) + ')'

    Set @sSQL = @sSQL + @sql
    IF @MaxRowsToFetch<>-1
    BEGIN
    SET @sSQL = @sSQL +  CHAR(13) + CHAR(10) + 'SET ROWCOUNT 0' 
    SET @sSQL = @sSQL +  CHAR(13) + CHAR(10) + 'SET NOCOUNT OFF'
    END

    EXEC (@sSQL)

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
