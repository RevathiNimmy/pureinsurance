SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Is_Policy_Paid_B'
GO


CREATE PROCEDURE spu_ACT_Is_Policy_Paid_B  
    @insurance_file_cnt INT,
    @document_id INT
AS  
  
BEGIN  
	SELECT Sum(t.amount) as amount,
		Sum(t.outstanding_amount) as outstanding_amount
	FROM Credit_Control_Item cci  
		INNER JOIN TransDetail t ON 
			t.account_id = cci.account_id  
	
	WHERE cci.insurance_file_cnt = @insurance_file_cnt
	AND cci.document_id = @document_id
	AND t.document_id = cci.document_id  
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
