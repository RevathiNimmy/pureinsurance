DDLDropProcedure 'spu_STS_FindPolicies'
GO

CREATE PROCEDURE spu_STS_FindPolicies
    @date_of_loss datetime = NULL,
    @policy_number varchar(30) = NULL,
    @client_surname varchar(40) = NULL,
    @client_postcode varchar(20) = NULL,
    @agent_name varchar(40) = NULL,
    @branch_code varchar(10)
AS
BEGIN
DECLARE @sSQL nvarchar(4000)
DECLARE @bNeedsWhere tinyint
DECLARE @bNeedsAnd tinyint
DECLARE @source_id int
    SELECT @source_id = source_id
    FROM   source
    WHERE  code = @branch_code
    IF (@policy_number IS NULL)
    BEGIN
        SET NOCOUNT ON
        SELECT @bNeedsWhere = 1
        SELECT @bNeedsAnd = 0
        SELECT @bNeedsAnd = 0

        SELECT @sSQL = 'select i.insurance_file_cnt, i.insured_cnt, (isnull(ifs.code , ''' + 'LIVE' +''')) as status,ift.code as type, i.insurance_ref as reference FROM insurance_file i '
        SELECT @sSQL = @sSQL + 'INNER JOIN party p ON p.party_cnt = i.insured_cnt '
        SELECT @sSQL = @sSQL + 'LEFT OUTER join insurance_file_status ifs on i.insurance_file_status_id=ifs.insurance_file_status_id '
        SELECT @sSQL = @sSQL + 'INNER JOIN insurance_file_type ift on i.insurance_file_type_id=ift.insurance_file_type_id '

        IF (@client_postcode IS NOT NULL)
            SELECT @sSQL = @sSQL + ' INNER JOIN address a ON a.postal_code = ''' + @client_postcode + ''' INNER JOIN party_address_usage pau ON pau.address_cnt = a.address_cnt '
        IF (@agent_name IS NOT NULL)
            SELECT @sSQL = @sSQL + ' INNER JOIN party p2 ON p2.party_cnt = i.lead_agent_cnt '
        IF (@client_surname IS NOT NULL)
        BEGIN
            IF (@bNeedsWhere = 1)
            BEGIN
                SELECT @sSQL = @sSQL + ' WHERE '
                SELECT @bNeedsWhere = 0
            END
            IF (@bNeedsAnd = 1)
                SELECT @sSQL = @sSQL + ' AND '
            SELECT @sSQL = @sSQL + ' p.name = ''' + @client_surname + ''' '
            SELECT @bNeedsAnd = 1
        END
        IF (@client_postcode IS NOT NULL)
        BEGIN
            IF @bNeedsWhere = 1
            BEGIN
                SELECT @sSQL = @sSQL + ' WHERE '
                SELECT @bNeedsWhere = 0
            END
            IF (@bNeedsAnd = 1)
                SELECT @sSQL = @sSQL + ' AND '
            SELECT @sSQL = @sSQL + ' pau.party_cnt = i.insured_cnt'
            SELECT @bNeedsAnd = 1
        END
        IF (@agent_name IS NOT NULL)
        BEGIN
            IF @bNeedsWhere = 1
            BEGIN
                SELECT @sSQL = @sSQL + ' WHERE '
                SELECT @bNeedsWhere = 0
            END
            IF (@bNeedsAnd = 1)
                SELECT @sSQL = @sSQL + ' AND '
            SELECT @sSQL = @sSQL + ' p2.shortname = ''' + @agent_name + ''' '
            SELECT @bNeedsAnd = 1
        END
        IF (@date_of_loss IS NOT NULL)
        BEGIN
            IF @bNeedsWhere = 1
            BEGIN
                SELECT @sSQL = @sSQL + ' WHERE '
                SELECT @bNeedsWhere = 0
            END
            IF (@bNeedsAnd = 1)
                SELECT @sSQL = @sSQL + ' AND '
            SELECT @sSQL = @sSQL + ' i.cover_start_date <= ''' + CONVERT(varchar(10), @date_of_loss, 120) + ''' AND i.expiry_date >=  ''' + CONVERT(varchar(10), @date_of_loss, 120) + ''''
            SELECT @bNeedsAnd = 1
        END
        IF (@bNeedsWhere = 1)
        BEGIN
            SELECT @sSQL = @sSQL + ' WHERE '
            SELECT @bNeedsWhere = 0
        END
        IF (@bNeedsAnd = 1)
            SELECT @sSQL = @sSQL + ' AND '
        SELECT @sSQL = @sSQL + ' i.source_id = ' + cast(@source_id as nvarchar(8)) 
        SET NOCOUNT OFF
        EXECUTE sp_executesql @sSQL
    END
    ELSE
    BEGIN
        --policy number has been passed
        SELECT @sSQL = 'select insf.insurance_file_cnt, insf.insured_cnt, (isnull(ifs.code , ''' + 'LIVE' +''')) as status,ift.code as type ,insf.insurance_ref as reference '
        SELECT @sSQL = @sSQL + 'FROM insurance_file insf left outer join insurance_file_status ifs on insf.insurance_file_status_id=ifs.insurance_file_status_id inner join insurance_file_type ift on insf.insurance_file_type_id=ift.insurance_file_type_id '
        SELECT @sSQL = @sSQL + 'WHERE  insf.insurance_ref '

        --if % then use like
        if (select CHARINDEX ('%',@policy_number)) > 0
            SELECT @sSQL = @sSQL + ' like'
        ELSE
            SELECT @sSQL = @sSQL + '='

        SELECT @sSQL = @sSQL + ' ''' + @policy_number + ''' AND insf.source_id =' + cast(@source_id as nvarchar(8)) 

        --check if we need to check for dates
        IF (@date_of_loss IS NOT NULL)
            SELECT @sSQL = @sSQL + ' AND insf.cover_start_date <= ''' + CONVERT(varchar(10), @date_of_loss, 120) + ''' AND insf.expiry_date >=  ''' + CONVERT(varchar(10), @date_of_loss, 120) + ''''
        SET NOCOUNT OFF
        EXECUTE sp_executesql @sSQL
    END
END

GO
