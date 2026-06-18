SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_accounthandler'
GO


CREATE PROCEDURE spu_wp_accounthandler
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

	SELECT	p.name account_handler_surname,
			p.resolved_name account_handler_name,
			pah.party_title_code account_handler_title,
			pah.forename account_handler_forename,
			pah.initials account_handler_initials,
			(
				SELECT d.description
				FROM department d
				WHERE d.department_id = pah.department_id
			) account_handler_department
			
	FROM	insurance_file i 
	JOIN	party p
	ON		p.party_cnt = i.account_handler_cnt 
	JOIN	party_account_handler pah
	ON		pah.party_cnt = p.party_cnt
	WHERE	i.insurance_file_cnt= @InsuranceFileCnt
			
GO


