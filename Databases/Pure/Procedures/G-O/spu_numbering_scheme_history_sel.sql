--Start(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (6.2.3)
  
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_numbering_scheme_history_sel'
Go

CREATE PROCEDURE spu_numbering_scheme_history_sel
	@policy_cnt int,
	@numbering_scheme_id int
As 
Begin
DECLARE @InsuranceFileStatusId INT,
        @InsuranceFileCnt INT
        
SELECT @InsuranceFileStatusId = insurance_file_status_id 
FROM Insurance_File(NOLOCK)	WHERE insurance_file_cnt = @policy_cnt

IF @InsuranceFileStatusId = 3
BEGIN
	  SELECT @InsuranceFileCnt = MIN(insurance_file_cnt)  
	  FROM insurance_file(NOLOCK) WHERE insurance_file_type_id = 2
	  AND insurance_folder_cnt = ( SELECT insurance_folder_cnt   
                                   FROM Insurance_File(NOLOCK) WHERE insurance_file_cnt = @policy_cnt )	

	  SELECT TOP 1
  		mask_code
	  FROM
  		numbering_scheme_history
	  WHERE
  		numbering_scheme_id = @numbering_scheme_id 
  		and Cast(Convert(VArchar,scheme_valid_from,101)As dateTime) <= (SELECT cover_start_date FROM Insurance_File(NOLOCK)
  			WHERE insurance_file_cnt = @InsuranceFileCnt)
	  ORDER BY
  		scheme_valid_from DESC
END
ELSE
BEGIN
	  SELECT TOP 1
  		mask_code
	  FROM
  		numbering_scheme_history
	  WHERE
  		numbering_scheme_id = @numbering_scheme_id and Cast(Convert(VArchar,scheme_valid_from,101)As dateTime) <= (SELECT cover_start_date FROM Insurance_File(NOLOCK)
  			WHERE insurance_file_cnt = @policy_cnt)
	  ORDER BY
  		scheme_valid_from DESC
END  		
End

--End(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (6.2.3)