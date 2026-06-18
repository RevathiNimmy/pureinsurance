SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_PartyBG_Details_Upd'
GO

CREATE PROCEDURE spu_PartyBG_Details_Upd
  @BG_id   INT,
  @bank_name_id  INT,
  @bank_branch  VARCHAR(50),
  @BG_ref   VARCHAR(50),
  @BG_currency_Id  SMALLINT,
  @BG_limit  NUMERIC(20,2),
  @available_bal  NUMERIC(20,2),
  @custody_branch_id INT,
  @issue_date DATETIME,
  @expiry_date  DATETIME,
  @is_policy_lock  TINYINT

AS

UPDATE bank_guarantee
SET
  bank_name_id = @bank_name_id,
  bank_branch = @bank_branch,
  BG_ref = @BG_ref,
  BG_currency_Id = @BG_currency_Id,
  BG_limit = @BG_limit,
  available_bal = @available_bal,
  expiry_date = @expiry_date,
  is_policy_lock = @is_policy_lock,
  Custody_branch_id = @custody_branch_id,
  Issue_date = @issue_date
WHERE Bg_id = @BG_id


SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO