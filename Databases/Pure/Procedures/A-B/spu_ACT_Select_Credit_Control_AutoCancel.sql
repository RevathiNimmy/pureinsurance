SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Select_Credit_Control_AutoCancel'
GO



CREATE PROCEDURE spu_ACT_Select_Credit_Control_AutoCancel
    @credit_control_item_id INT
AS
    SELECT
        A.account_key,
        I.insurance_folder_cnt
    FROM
        Credit_Control_Item CCI
    JOIN
        Account A ON A.account_id=CCI.account_id
    JOIN
        Insurance_File I ON I.insurance_file_cnt=CCI.insurance_file_cnt
    WHERE
        CCI.credit_control_item_id=@credit_control_item_id



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
