SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Add_RenewalReport'
GO


CREATE PROCEDURE spu_Add_RenewalReport
    @report_type varchar(30),
    @client_name varchar(60),
    @policy_number varchar(30),
    @agent_code varchar(60),
    @cover_start_date datetime,
    @cover_end_date datetime,
    @product_code varchar(255),
    @failure_criterion varchar(255),
    @failure_detail varchar(255),
    @user_id int,
    @insurance_file_cnt int = NULL   
AS    
  IF @insurance_file_cnt = 0 BEGIN  
  	SET @insurance_file_cnt = NULL  
  END  


INSERT INTO Renewal_Report
(
    report_type,
    client_name,
    policy_number,
    agent_code,
    cover_start_date,
    cover_end_date,
    product_code,
    failure_criterion,
    failure_detail,
    user_id,
	insurance_file_cnt
)
VALUES
(
    @report_type,
    @client_name,
    @policy_number,
    @agent_code,
    @cover_start_date,
    @cover_end_date,
    @product_code,
    @failure_criterion,
    @failure_detail,
    @user_id,
	@insurance_file_cnt
)

GO


