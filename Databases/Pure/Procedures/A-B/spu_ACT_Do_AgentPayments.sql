SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_AgentPayments'
GO

CREATE PROCEDURE spu_ACT_Do_AgentPayments
    @account_id int,
    @date_to datetime = NULL,
    @date_by_trans bit = 0,
    @marked_status int = NULL,
    @month int = NULL
AS
/****** Object:  Stored Procedure dbo.sp_ACT_Do_AgentPayments    Script Date: 16/10/00 12:03:16 ******/
/******************************************************************************************/
/*                                            */
/* Procedure: sp_ACT_Do_AgentPayments                         */
/*                                            */
/* Description: Used by AgentPayments                         */
/*                                            */
/* Edit History: ECK 130300 - Created.                            */
/*               DC  021000 - Obtained info on Client side of transaction                 */
/*       .                                    */
/*      ECK 230102 - Get client outstanding from sub agent if present        */
/*      eck210302 - Date check by Transaction or Effective            */
/*      eck210302 - Replace use of base_match with currency_match amounts         */
/*      eck300502 expand to date to include whole day to midnight       */
/*      eck300502 update the insurance_ref for Binder journals          */
/*      DJM 04/07/2002 : Amount settled now includes agent adj */
/*      MKW 030403 : Applied changes from 1.6.9 Sirius Sourcesafe CATCHUP */
/******************************************************************************************/

BEGIN

DECLARE @settlement_period  smallint
DECLARE @amt_settled        numeric(19, 4) 
DECLARE @transdetail_id     int
 
DECLARE @document_id        int
DECLARE @currency_amount    numeric(19, 4)
DECLARE @tax_amount         numeric(19, 4)
DECLARE @commadj_amount     numeric(19, 4)
DECLARE @commadj_tax_amount numeric (19,4)
DECLARE @document_id_copy   int   
DECLARE @commadj_trans      varchar(255)    
/* DC 021000 added following declarations for info on client side of transaction */
DECLARE @client_amount      numeric(19, 4)
DECLARE @client_transdetail_id  int
DECLARE @client_settled     numeric(19, 4)
--DC161101
DECLARE @client_name varchar(30)
DECLARE @short_code varchar(20)

/*eck300502 expand to date to include whole day to midnight */
    IF @date_to is not NULL
    BEGIN
        select @date_to = dateadd(hh,23,@date_to)
        select @date_to = dateadd(mi,59,@date_to)       
        select @date_to = dateadd(ss,59,@date_to)
    END
 
/* DC 021000 */   
/* Get the settlement period */
    SELECT  @settlement_period = settlement_period
    FROM    Account
    WHERE   account_id = @account_id

    CREATE TABLE #InsurerTemp
    (
        account_name CHAR(60),
        insurer_ref VARCHAR(30),
        document_ref VARCHAR(25),
        gross_transdetail_id INT,
        gross_amount MONEY,
        primary_transdetail_id  INT,   
        primary_amount MONEY,
        adj_transdetail_id VARCHAR(255),
        adj_amount MONEY,
        fee_transdetail_id INT,
        fee_amount MONEY,
        amt_settled MONEY,
        document_id INT,
        accounting_date DATETIME,
        currency_id SMALLINT,
        marked_status TINYINT,
        month SMALLINT,
        spare CHAR(8),
        payment MONEY,
        source_id INT,
        short_code CHAR(20),
        client_transdetail_id INT,
        client_amount MONEY,
        client_settled MONEY,
        period VARCHAR(15),
	tax money 
    )

    INSERT INTO #InsurerTemp

    --DC161101 set account_name later
    SELECT  '', --a2.account_name,
        t.insurance_ref,
        d.document_ref,
        t.transdetail_id,
        t.currency_amount,
        0,
        t2.currency_amount,
        '',
        0,
        0,
        0,
        (
                    SELECT isnull(sum(aa.alloc_ccy_amount),0)
                    FROM   transdetail tt,
                           allocationdetail aa
                    WHERE   tt.document_id = t.document_id
                    AND tt.account_id = t.account_id
                    AND    aa.transdetail_id = tt.transdetail_id
                ),
        d.document_id,
        t.accounting_date,
        t.currency_id,
        (
                    SELECT isnull(sum(0)+1,0)
                    FROM   transmatch tm
                    WHERE  tm.allocationdetail_id IS null
                    AND    tm.transdetail_id = t.transdetail_id
                ),
        DatePart(mm, DateAdd(dd, @settlement_period,t.accounting_date)),
        t.spare,
        0,
        t.company_id,
        a2.short_code,
        0,
        0,
        0,
        p.period_name,
 	t.ref_amount

        FROM  Account a
        JOIN  Transdetail t
        ON    t.account_id = a.account_id
        JOIN  Document d
        ON    d.document_id = t.document_id
        JOIN  period p
        ON    p.period_id = t.period_id
        JOIN  Transdetail t2
        ON    t2.document_id = t.document_id
        JOIN  Account a2
        ON    a2.account_id = t2.account_id
     
    WHERE   (a.account_id = @account_id)
    AND     (t2.document_sequence = 1)
    AND ((t.spare= 'AGENT')OR (t.spare = '' AND  d.documenttype_id = 1 ))

    AND (
            (   SELECT  sum(currency_amount)
                FROM    transdetail
                WHERE   document_id = t.document_id
                AND account_id = t.account_id
            ) <> 
            (   SELECT  isnull(sum(aa.alloc_ccy_amount),0)          
                FROM    transdetail tt,
                    allocationdetail aa    
                WHERE   tt.document_id = t.document_id
                AND tt.account_id = t.account_id
                AND aa.transdetail_id = tt.transdetail_id
            )
        OR
        
            (
                (   SELECT  sum(currency_amount)
                    FROM    transdetail
                    WHERE   document_id = t.document_id
                    AND account_id = t.account_id
                ) = 0
            AND
                NOT EXISTS
                (   SELECT  aa.allocationdetail_id          
                    FROM    transdetail tt,
                        allocationdetail aa    
                    WHERE   tt.document_id = t.document_id
                    AND tt.account_id = t.account_id
                    AND aa.transdetail_id = tt.transdetail_id
                )
            )
        
        )
        
    /*Don't include ones that are out of the date range*/
    IF @date_to IS NOT NULL
    BEGIN
        IF @date_by_trans = 0
        BEGIN
            DELETE FROM #InsurerTemp WHERE accounting_date > @date_to
        END
        ELSE
        BEGIN
            DELETE FROM #InsurerTemp
            WHERE document_id in
            (
                SELECT i.document_id
                FROM #InsurerTemp i
                JOIN document d
                ON d.document_id = i.document_id
                WHERE d.created_date > @date_to
            )
        END
    END

    /*Don't include ones that dont comply with the month, if needed*/
    IF @month IS NOT NULL
    BEGIN 
        DELETE FROM #InsurerTemp WHERE month <> @month
    END

    /*Don't include the marked ones*/   
    IF @marked_status IS NOT NULL
    BEGIN
        DELETE FROM #InsurerTemp WHERE marked_status <> @marked_status
    END
    
/* eck update the insurance_ref for Binder journals */
    UPDATE  #InsurerTemp    
    SET insurer_ref  = d.comment
        FROM    transdetail t,document d,       
        #insurertemp it
    WHERE   it.gross_transdetail_id = t.transdetail_id
    AND t.document_id = d.document_id
    AND d.comment = 'Consolidated Binder'


    /* DC 021000 update info on client side of transaction */
    UPDATE  #InsurerTemp
    SET client_transdetail_id = t.transdetail_id,
        client_amount = t.currency_amount
    FROM    transdetail t, #insurertemp it
    WHERE   it.document_id = t.document_id
    AND t.document_sequence = 1
    AND t.spare = ''
    /* DC 021000 */
-- ECK 230102 update info on client side of transaction with sub agent if nessessary
    UPDATE  #InsurerTemp
    SET client_transdetail_id = t.transdetail_id,
        client_amount = t.currency_amount
    FROM    transdetail t, #insurertemp it,
        account a, ledger l
    WHERE   it.document_id = t.document_id
    AND t.account_id = a.account_id
    AND a.ledger_id = l.ledger_id
    AND     l.ledger_short_name = 'UB'
 -- ECK 230102
        --Client 
        update #InsurerTemp
        set    client_settled = (select isnull(sum(tm.currency_match_amount),0)
        from   transdetail t, transmatch tm
        where  t.transdetail_id = client_transdetail_id
        and    tm.transdetail_id = t.transdetail_id)

        --account name from party table 
        update #InsurerTemp
        set    account_name = left(isnull(resolved_name,''),60)
        from   party p
        where  p.shortname = short_code

        --account name from account table
        update #InsurerTemp
        set    #InsurerTemp.account_name = isnull(a.account_name,'')
        from   account a
        where  a.short_code = #InsurerTemp.short_code
        and    #InsurerTemp.account_name = ''

/*Commission Adjustments */

/* There could be more than one so need a SCROLL CURSOR */
/* Declare the Temp Cursor */
    
    SELECT @commadj_amount = 0
    SELECT @commadj_trans = ''
    SELECT @amt_settled = 0 -- DJM 04/07/2002 : Amount settled now includes agent adj
    SELECT @commadj_tax_amount = 0    
    DECLARE 
        it_adjtemp CURSOR FORWARD_ONLY FOR SELECT
        currency_amount = t.currency_amount,
        document_id = it.document_id ,
        transdetail_id = t.transdetail_id,
	ref_amount = round(t.ref_amount,2)
    FROM    transdetail t,
        #InsurerTemp it 
    WHERE   it.document_id = t.document_id
    AND     t.account_id = @account_id
    AND t.spare = 'AGENT ADJ'
    ORDER BY it.document_id 
/* Open the temp Cursor */
 
    OPEN it_adjtemp

    FETCH NEXT FROM it_adjtemp INTO @currency_amount,
                     @document_id,
                     @transdetail_id,
		     @tax_amount

    
    SELECT @document_id_copy = @document_id

    WHILE (@@FETCH_STATUS = 0) 
        BEGIN   
            -- Are we still on the same doc_id?
            IF @document_id_copy = @document_id  
                Begin
                    -- Add the curr amnts together for the same doc id   
                    SELECT @commadj_amount = @commadj_amount + @currency_amount
                    SELECT @commadj_trans = @commadj_trans + CONVERT(varchar,@transdetail_id) + '|' 
            	    SELECT @commadj_tax_amount = @commadj_tax_amount + @tax_amount                   

                End
            /* Fetch Next */
            FETCH NEXT FROM it_adjtemp 
            INTO    @currency_amount,
                @document_id,
                @transdetail_id,
		@tax_amount
            
            IF @document_id_copy <> @document_id  
                Begin
                    UPDATE  #InsurerTemp                    
                    SET  adj_amount = @commadj_amount,                  
                         adj_transdetail_id = @commadj_trans, 
 			 tax = tax + @commadj_tax_amount
                    WHERE   document_id = @document_id_copy 
                    SELECT @commadj_amount = 0
		    SELECT @commadj_tax_amount = 0
                    SELECT @commadj_trans = ''
                    SELECT @document_id_copy = @document_id
                End
        END   
    
    -- Any left over?           
    IF @commadj_amount <> 0  
        Begin
            UPDATE  #InsurerTemp            
            SET adj_amount = @commadj_amount,
                adj_transdetail_id = @commadj_trans,
		tax = tax  + @commadj_tax_amount 
            WHERE   document_id = @document_id
        End

    /* Close and Deallocate Cursor */
    CLOSE it_adjtemp
    
    DEALLOCATE it_adjtemp  

 

--eck210302 get currency match amount not base match amount
    UPDATE  #InsurerTemp    
    SET payment = tm.currency_match_amount  
    FROM    transmatch tm   
    WHERE   gross_transdetail_id = tm.transdetail_id
    AND tm.allocationdetail_id IS null 
 
    /* Select it all. We know the column order so an asterix should suffice. */
    SELECT      *   
        FROM        #InsurerTemp
        ORDER BY    source_id, insurer_ref, document_ref
    /* Remove the temp table */  

    
    DROP TABLE #InsurerTemp  


END

GO


