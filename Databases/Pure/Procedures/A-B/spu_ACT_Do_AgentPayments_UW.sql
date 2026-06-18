SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_AgentPayments_UW'
GO

-- Changes:
-- TB230403 $Revision: 4 $ Return the MediaRef column from CashListItem
--          Also Put blank line after each statement for legibility
-- 

CREATE PROCEDURE spu_ACT_Do_AgentPayments_UW
    @account_id int,
    @date_to datetime = NULL,
    @date_by_trans bit = 0,
    @marked_status int = NULL,
    @month int = NULL
AS

BEGIN
DECLARE @settlement_period  smallint
DECLARE @amt_settled        numeric(19, 4)
DECLARE @transdetail_id     int
DECLARE @document_id        int
DECLARE @document_id_copy   int
DECLARE @currency_amount    numeric(19, 4)
DECLARE @commadj_amount     numeric(19, 4)
DECLARE @commadj_trans      varchar(255)
DECLARE @client_amount      numeric(19, 4)
DECLARE @client_transdetail_id  int
DECLARE @client_settled     numeric(19, 4)
DECLARE @gross_transdetail_id   int,
        @tax_transdetail_id     int,
        @tax_document_id        int,
        @tax_amount             numeric(19,4),
        @InstalmentStatus       varchar(3)
DECLARE @OverAllocate_Amount numeric(19,4),
    @GrossLine numeric(19,4)

    SELECT  @settlement_period = settlement_period
    FROM    Account
    WHERE   account_id = @account_id

    CREATE TABLE #InsurerTemp
    (
        account_name            char(100),
        insurer_ref             varchar(30) NULL,
        document_ref            varchar(25) NULL,
        gross_transdetail_id    int NULL,
        gross_amount            numeric(19, 4) NULL,
        comm_transdetail_id     int NULL,
        comm_amount             numeric(19, 4) NULL,
        commadj_transdetail_id  varchar(255),
        commadj_amount          numeric(19, 4) NULL,
        amt_settled             numeric(19, 4) NULL,
        document_id             int NULL,
        accounting_date         datetime NULL,
        currency_id             smallint NULL,
        marked_status           tinyint,
        month                   smallint NULL,
        spare                   char(20),
        payment                 numeric(19, 4) NULL,
        source_id               int,
        short_code              char(20),
        client_transdetail_id   int NULL,
        client_amount           numeric(19, 4) NULL,
        client_settled          numeric(19, 4) NULL,
        period                  varchar(15) NULL,
        InstalmentStatus        varchar(3),
        tax_amount              numeric(19,4) NULL,
        tax_transdetail_id      int NULL,
		media_ref				varchar(100) NULL
	)

        INSERT INTO #InsurerTemp
        SELECT  a2.account_name,
            t.insurance_ref,
            d.document_ref,
            t.transdetail_id,
            t.currency_amount,
            0,
            0,
            '',
            0,
            0,
            d.document_id,
            t.accounting_date,
            t.currency_id,
            0,
            DatePart(mm, DateAdd(dd, @settlement_period, t.accounting_date)),
            t.spare,
            0,
            t.company_id,
            a2.short_code,
            0,
            0,
            0,
            p.period_name,
            0,
            0,
            0,
			CLI.Media_Ref
        FROM    Account a, Account a2, Transdetail t2, 
				Document d , period P, Transdetail t
		LEFT OUTER JOIN CashListItem CLI 
		ON t.transdetail_id = CLI.transdetail_id			
        WHERE   (a.account_id = @account_id)
        AND (t.account_id = @account_id)
        AND (a2.account_id = t2.account_id)
        AND (d.document_id = t.document_id)
        AND ((t.accounting_date <= @date_to and @date_by_trans = 0)OR (d.created_date <= @date_to  and @date_by_trans = 1)OR @date_to IS NULL)
        AND ((t.spare = 'GROSS' AND t2.document_sequence = 1)
            OR (t.spare = '' AND  d.documenttype_id = 1 AND t2.document_sequence = 1)
            OR ( t.spare = '' AND (d.documenttype_id = 33 OR d.documenttype_id = 34) AND t2.account_id <> t.account_id)
            OR (d.documenttype_id in (SELECT documenttype_id FROM DocumentType WHERE code in ('CLP','CLR','JN')) AND t2.document_sequence = 1)
            OR (d.documenttype_id in (SELECT documenttype_id FROM DocumentType WHERE code in ('SRP', 'SPY')) AND t2.document_sequence = 1 AND IsNull(t2.fully_matched,0) = 0))
        AND     (t.document_id = t2.document_id)
        AND (p.period_id = t.period_id)

--    DELETE #InsurerTemp FROM #InsurerTemp INNER JOIN CashListItem
--        ON gross_transdetail_id = transdetail_id
--        WHERE   Left(document_ref,3) IN ('SRP','SPY')
--        AND     AllocationStatus_id IN (SELECT allocationstatus_id FROM AllocationStatus WHERE code IN ('A'))

    DECLARE
        c_it_temp SCROLL CURSOR FOR SELECT
        gross_transdetail_id,
        document_id
    FROM    #InsurerTemp

    OPEN c_it_temp

    FETCH FIRST FROM c_it_temp INTO
            @gross_transdetail_id,
            @tax_document_id

    WHILE (@@FETCH_STATUS = 0)
    BEGIN
		SELECT	@InstalmentStatus = '000'
        SELECT  @tax_amount = 0
        SELECT  @tax_transdetail_id = 0
        SELECT  @tax_amount = currency_amount,
                @tax_transdetail_id = transdetail_id
        FROM    transdetail
        WHERE   document_id = @tax_document_id
        AND     account_id = @account_id
        AND     spare like 'TAX%'
        SELECT  @InstalmentStatus = pf.StatusInd
        FROM    PFTransaction_Id pt INNER JOIN PFPremiumFinance pf
        ON      pt.pfprem_finance_cnt = pf.pfprem_finance_cnt AND pt.pfprem_finance_version = pf.pfprem_finance_version
        WHERE   pt.pftransaction_id = @gross_transdetail_id
        SELECT @InstalmentStatus = IsNull(@InstalmentStatus,'000')
        UPDATE  #InsurerTemp
        SET     gross_amount  = gross_amount + @tax_amount,
                InstalmentStatus = @InstalmentStatus,
                tax_transdetail_id = @tax_transdetail_id,
                tax_amount = @tax_amount
        WHERE   document_id = @tax_document_id
        AND     gross_transdetail_id = @gross_transdetail_id
        FETCH NEXT FROM c_it_temp INTO
                @gross_transdetail_id,
                @tax_document_id
    END

    CLOSE c_it_temp

    DEALLOCATE c_it_temp

    UPDATE  #InsurerTemp
	    SET comm_transdetail_id  = t.transdetail_id,
        comm_amount = t.currency_amount
        FROM    transdetail t,
        #insurertemp it
    WHERE   it.document_id = t.document_id
    AND t.account_id = @account_id
    AND t.spare = 'COMM'

    UPDATE  #InsurerTemp
	    SET client_transdetail_id = t.transdetail_id,
        client_amount = t.currency_amount
    FROM    transdetail t, #insurertemp it
    WHERE   it.document_id = t.document_id
    AND t.document_sequence = 1
    AND t.spare = ''

    UPDATE  #InsurerTemp
	    SET     account_name = p.resolved_name,
             short_code = p.shortname
    FROM    Insurance_File ifi,
             Insurance_Folder ifo,
             Party p,
             #InsurerTemp it
    WHERE   ifi.insurance_ref = it.insurer_ref
    AND     ifo.insurance_folder_cnt = ifi.insurance_folder_cnt
    AND     p.party_cnt = ifo.insurance_holder_cnt

    DECLARE
        it_instemp SCROLL CURSOR FOR SELECT
        transdetail_id = it.gross_transdetail_id,
        client_transdetail_id = it.client_transdetail_id
    FROM    #InsurerTemp it

    OPEN it_instemp

    FETCH FIRST FROM it_instemp INTO
        @transdetail_id, @client_transdetail_id

    WHILE (@@FETCH_STATUS = 0)
    BEGIN
        SELECT @amt_settled = 0
	    SELECT  @amt_settled = SUM(ad.alloc_ccy_amount)
    	FROM    allocationdetail ad,
        	    transdetail td
	    WHERE   ad.document_ref IN
    	        (
        	    SELECT  document_ref
            	FROM    allocationdetail
		        WHERE   transdetail_id = @transdetail_id
        	    )
	    AND     ad.transdetail_id = td.transdetail_id
	    AND     td.account_id = @account_id
	    AND     ISNULL(td.spare,'') IN
    	        (
        	    'COMM',
	            'TAX',
	            (SELECT ISNULL(spare,'') FROM transdetail WHERE transdetail_id = @transdetail_id))
            
	-- Thinh Nguyen 14/03/2003 (start) - if its a cash line we'll need to link via cashlistitem_id
	IF IsNull(@amt_settled,0) = 0
	BEGIN
		SELECT	@amt_settled = SUM(tm.currency_match_amount)
		FROM	Transdetail td 	INNER JOIN Transmatch tm ON td.transdetail_id = tm.transdetail_id
		WHERE	td.transdetail_id = @transdetail_id
		AND	tm.allocationdetail_id IS NOT NULL
		
	END
	-- Thinh Nguyen 14/03/2003 (stop) - if its a cash line we'll need to link via cashlistitem_id
            
            
            
        UPDATE  #InsurerTemp
            SET amt_settled = @amt_settled
            WHERE   gross_transdetail_id =  @transdetail_id
            AND @amt_settled IS NOT NULL
    SELECT  @amt_settled = SUM(amount)
    FROM    transdetail td
    WHERE   td.document_id =
            (
            SELECT  document_id
            FROM    transdetail
            WHERE   transdetail_id = @transdetail_id
            )
    AND    td.spare = 'ALLOCATED'
    AND     td.account_id = @account_id
        UPDATE  #InsurerTemp
            SET amt_settled = amt_settled - @amt_settled
            WHERE   gross_transdetail_id =  @transdetail_id
            AND @amt_settled IS NOT NULL
        SELECT  @client_settled = 0
    SELECT  @client_settled = sum(a.alloc_ccy_amount)
        FROM    transdetail t, allocationdetail a
        WHERE   t.transdetail_id = @client_transdetail_id
        AND a.transdetail_id = t.transdetail_id
        UPDATE  #InsurerTemp
        SET client_settled = @client_settled
        WHERE   gross_transdetail_id = @transdetail_id
        AND     @client_settled IS NOT NULL
        FETCH NEXT FROM it_instemp
        INTO    @transdetail_id, @client_transdetail_id
    END

    CLOSE it_instemp

    DEALLOCATE it_instemp

    DELETE FROM #InsurerTemp
    WHERE       amt_settled = gross_amount + comm_amount

    IF (@month IS NOT NULL)
    BEGIN
        DELETE FROM #InsurerTemp
        WHERE       month <> @month
    END

    UPDATE  #InsurerTemp    SET marked_status = 1
    WHERE   gross_transdetail_id IN (
	    SELECT  transdetail_id
    	        base_match_amount
	    FROM    transmatch
	    WHERE   allocationdetail_id IS null)

    UPDATE  #InsurerTemp
    SET payment = tm.base_match_amount
    FROM    transmatch tm
    WHERE   gross_transdetail_id = tm.transdetail_id
    AND tm.allocationdetail_id IS null    and tm.base_match_amount  >=   gross_amount + comm_amount + commadj_amount
    AND gross_amount < 0

    UPDATE  #InsurerTemp
    SET payment = tm.base_match_amount
    FROM    transmatch tm
    WHERE   gross_transdetail_id = tm.transdetail_id
    AND tm.allocationdetail_id IS null    and tm.base_match_amount  <=   gross_amount + comm_amount + commadj_amount
    AND gross_amount > 0

    UPDATE  #InsurerTemp
    SET payment = ( gross_amount + comm_amount + commadj_amount) - amt_settled
    FROM    transmatch tm
    WHERE   gross_transdetail_id = tm.transdetail_id
    AND tm.allocationdetail_id IS null   and tm.base_match_amount  <   (gross_amount + comm_amount + commadj_amount ) - amt_settled
    AND  gross_amount < 0

    UPDATE  #InsurerTemp
    SET payment = ( gross_amount + comm_amount + commadj_amount) - amt_settled
    FROM    transmatch tm
    WHERE   gross_transdetail_id = tm.transdetail_id
    AND tm.allocationdetail_id IS null   and tm.base_match_amount >   (gross_amount + comm_amount + commadj_amount ) - amt_settled
    AND  gross_amount > 0

    UPDATE  #InsurerTemp
    SET commadj_transdetail_id  = t.transdetail_id,
        commadj_amount = t.currency_amount
        FROM    transdetail t,
        #insurertemp it
    WHERE   it.document_id = t.document_id
    AND t.spare = 'COMM ADJ'

    IF (@marked_status IS NOT NULL)
    BEGIN
        DELETE FROM #InsurerTemp
        WHERE       marked_status = (1 - (@marked_status))
    END

    UPDATE  #InsurerTemp
    SET amt_settled = 0
    WHERE   amt_settled IS null

    SELECT  *
    FROM        #InsurerTemp
    ORDER BY source_id, insurer_ref, document_ref

    DROP TABLE #InsurerTemp
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

