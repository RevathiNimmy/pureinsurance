SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Preferred_Correspondence_Address'
GO
CREATE PROCEDURE spu_Get_Preferred_Correspondence_Address
	@insurance_file_cnt int,
	@document_template_id int
 AS 
 BEGIN
		DECLARE @CORRESPONDENCE_TYPE varchar(50)
		DECLARE @DEFAULT_PREFERRED_CORRESPONDENCE varchar(50)
		DECLARE @LEAD_AGENT varchar(50)
		 DECLARE @IS_AGENT_RECEIVE_CORRESPONDENCE TINYINT
  
		select @CORRESPONDENCE_TYPE=c2.code,
			   @DEFAULT_PREFERRED_CORRESPONDENCE=c1.code , 
		       @LEAD_AGENT = ifile.lead_agent_cnt,
			   @IS_AGENT_RECEIVE_CORRESPONDENCE = ifile.Is_Agent_Correspondence
		from insurance_file ifile
		left join Correspondence_Type c1 on ifile.Correspondence_Type = c1.Correspondence_Type_ID
		left join contact_type c2 on ifile.Default_Preferred_Correspondence = c2.contact_type_id
		where insurance_file_cnt =@insurance_file_cnt

		select 
			document_type_id,
			email_sub_template_code, 
			email_attachment_template_code,
			@CORRESPONDENCE_TYPE CORRESPONDENCE_TYPE,
			@DEFAULT_PREFERRED_CORRESPONDENCE DEFAULT_PREFERRED_CORRESPONDENCE, 
			@LEAD_AGENT lead_agent
	   from Document_Template 
	   where Document_Template_id =@document_template_id

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO