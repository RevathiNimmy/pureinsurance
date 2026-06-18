SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_do_balance_sheet_report'
GO

CREATE PROCEDURE spu_ACT_do_balance_sheet_report
    @sub_branch_id int
AS
/*******************************************************************************************/
/* Stored Procedure for the Orion Balance Sheet Report. */
/*******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 22/01/1999 RFC */
/*******************************************************************************************/

    DECLARE 
        @mapping_id integer,
        @node_id integer,
        @description varchar(255),
        @root_mapping char(2),
        @root_mapping_id integer,
        @root_node_id integer,
        @element_name varchar(20),
        @parent_mapping varchar(20),
        @current_period varchar(15),
        @current_year varchar(20),
        @current_period_end datetime

    /* TF050299 - Determine Current Period and Financial Year (based on General Ledger) */
    SELECT @current_period = period_name ,
           @current_year = year_name ,
           @current_period_end = period_end_date
    FROM   Ledger L
    JOIN   Period P ON P.period_id = L.current_period_id
    WHERE  L.sub_branch_id = @sub_branch_id

    CREATE TABLE #account_mapping (
        node_id integer ,
        mapping_id integer NULL ,
        mapping_description varchar(255) NULL ,
        root_mapping char(2) NULL ,
        parent_mapping varchar(20) NULL
    )

    DECLARE c_transdetail_accounts CURSOR FAST_FORWARD FOR
        SELECT DISTINCT 
               s.node_id
        FROM   Account A
        JOIN   Transdetail T ON A.account_id = T.account_id
        JOIN   StructureTree S ON A.account_id = S.account_id
        JOIN   Period P ON P.period_id = T.period_id
        WHERE  P.year_name = @current_year
        AND    P.period_end_date <= @current_period_end
        AND    A.sub_branch_id = @sub_branch_id

    OPEN c_transdetail_accounts
    FETCH NEXT FROM c_transdetail_accounts INTO @node_id

    WHILE @@FETCH_STATUS = 0 BEGIN
        EXECUTE spu_ACT_do_get_node_mapping
            @node_id = @node_id ,
            @mapping_id = @mapping_id OUTPUT

        SELECT @description = NULL
        SELECT @description = description
        FROM   mapping
        WHERE  mapping_id = @mapping_id

        /* TF040299 - Determine if Profit & Loss or Balance Sheet */
        SELECT 
            @root_node_id = @node_id ,
            @root_mapping_id = @mapping_id ,
            @parent_mapping = ''


        WHILE @root_node_id > 1 BEGIN
            SELECT @element_name = element_name ,
                   @root_node_id = parent_node_id
            FROM   Element E
            JOIN   StructureTree S ON E.element_id = S.element_id
            WHERE  S.node_id = @root_node_id

            /* Check for parent node (2nd level up for Balance Sheet report) */
            IF @parent_mapping = '2'
                SELECT @parent_mapping = @element_name

            IF @parent_mapping = '1'
                SELECT @parent_mapping = '2'

            IF @parent_mapping = '' 
                SELECT @parent_mapping = '1'

            IF @element_name = 'Sales Ledger' 
                SELECT @description = 'Debtors'

            IF @element_name = 'Purchase Ledger' 
                SELECT @description = 'Creditors'

            IF @element_name = 'Profit and Loss' 
            BEGIN
                SELECT @root_mapping = 'PL'
                BREAK
            END

            IF @element_name = 'Balance Sheet' 
            BEGIN
                SELECT @root_mapping = 'BS'
                BREAK
            END
        END

        INSERT INTO #account_mapping (
            node_id ,
            mapping_id ,
            mapping_description ,
            root_mapping ,
            parent_mapping
        ) VALUES (
            @node_id ,
            @mapping_id ,
            @description ,
            @root_mapping ,
            @parent_mapping
        )

        FETCH NEXT FROM c_transdetail_accounts INTO @node_id
    END

    CLOSE c_transdetail_accounts
    DEALLOCATE c_transdetail_accounts

    SET NOCOUNT OFF

    SELECT TransDetail.accounting_date,
           TransDetail.amount,
           Account.account_name,
           Account.short_code,
           Period.period_end_date,
           Period.period_end_complete,
           Company.code CompanyCode,
           #account_mapping.mapping_description ,
           #account_mapping.mapping_id ,
           #account_mapping.root_mapping ,
           #account_mapping.parent_mapping ,
           Period.period_name ,
           @current_year year_name ,
           @current_period current_period
    FROM   TransDetail
    JOIN   Document ON Document.document_id = TransDetail.document_id
    JOIN   Account ON TransDetail.account_id = Account.account_id
    JOIN   Period ON TransDetail.period_id = Period.period_id
    JOIN   Company ON Document.company_id = Company.company_id
    JOIN   AccountType ON Account.accounttype_id = AccountType.accounttype_id
    JOIN   DocumentType ON Document.documenttype_id = DocumentType.documenttype_id
    JOIN   StructureTree ON TransDetail.account_id = StructureTree.account_id
    JOIN   #account_mapping ON StructureTree.node_id = #account_mapping.node_id
    WHERE  Period.year_name = @current_year
    AND    Period.period_end_date <= @current_period_end

    ORDER BY CompanyCode, 
            #account_mapping.root_mapping, 
            #account_mapping.mapping_id


    DROP TABLE #account_mapping

GO

