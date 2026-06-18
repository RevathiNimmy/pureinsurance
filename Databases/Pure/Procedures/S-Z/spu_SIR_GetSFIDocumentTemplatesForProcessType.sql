--**********************************************************************************************  
-- Author : Pankaj Kaushik
--   
-- History: 04/03/2008    
--
-- Task : Renewal Printing
--**********************************************************************************************  
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_SIR_GetSFIDocumnetTemplatesForProcessType'
GO
CREATE PROCEDURE spu_SIR_GetSFIDocumnetTemplatesForProcessType(  
 @functional_area TINYINT,  
 @insurance_file_cnt INT,  
 @process_types_docs_id INT,  
 @process_type_code VARCHAR(20),  
 @effective_date DATETIME,  
 @called_from_SAM bit = 0)  
AS  
BEGIN  
  DECLARE @lead_agent_cnt int  
  DECLARE @process_types_docs varchar(255)  
  DECLARE @product_id int  
  DECLARE @source_id int  
  DECLARE @CORRESPONDENCE_TYPE varchar(50)
  DECLARE @DEFAULT_PREFERRED_CORRESPONDENCE varchar(50)
  DECLARE @IS_AGENT_RECEIVE_CORRESPONDENCE TINYINT  
  DECLARE @is_agent int,@is_office int,@production_order int,@spool_document int,@is_client int,@is_message int  
   
   select @product_id = product_id, @source_id = source_id, 
   @CORRESPONDENCE_TYPE=c2.code,
   @DEFAULT_PREFERRED_CORRESPONDENCE=c1.code,
   @IS_AGENT_RECEIVE_CORRESPONDENCE = Is_Agent_Correspondence
   from insurance_file ifile
   left join contact_type c1 on ifile.Default_Preferred_Correspondence  = c1.contact_type_id
   left join Correspondence_Type c2 on ifile.Correspondence_Type  = c2.Correspondence_Type_ID
   where insurance_file_cnt = @insurance_file_cnt

  SELECT @lead_agent_cnt = ISNULL(lead_agent_cnt,0) FROM insurance_file WITH (NOLOCK) WHERE  insurance_file_cnt = @insurance_file_cnt  
  SELECT @process_types_docs=description from process_types_docs where process_types_docs_id=@process_types_docs_id  
  Create table #tempCode  
   (id integer identity, code varchar(100))  
 Create table #tempDocuments  
  ( Document_Template_Id int,  
  Document_Type_Id int,  
  is_client tinyint,  
  is_agent tinyint,  
  is_office tinyint,  
  production_order tinyint,  
  lead_agent_cnt int,  
  description varchar(200),  
  spool_document tinyint,  
  is_editable_after_merging tinyint,  
  process_types_docs varchar(20),  
  is_message tinyint,
  Email_subject varchar(50),
  Email_attachment varchar(250) , 
  CORRESPONDENCE_TYPE varchar(50) , 
  DEFAULT_PREFERRED_CORRESPONDENCE varchar(50),
  IS_AGENT_RECEIVE_CORRESPONDENCE TINYINT,
  code varchar(100))  


  Declare @code Varchar(100)  
  Insert into #tempcode  
  Select DT.code from document_template DT  
   JOin pmb_doc_link PDL on PDL.document_template_id=Dt.document_template_id  
   Join process_type PT on PT.process_type_id=PDL.process_type_id  
   Join process_types_docs PTD on PTD.process_types_docs_id=PDL.process_types_docs_id  
  where PT.code=@process_type_code and PTD.process_types_docs_id=@process_types_docs_id  
   and pdl.product_id = @product_id  
   and (pdl.source_id = @source_id or pdl.source_id = 0 or pdl.source_id is null)  
  Declare @cntCode int  
  Select @cntCode = count(*) from #tempCode  
While @cntCode > 0  
BEGIN  
select  
  @is_client=pdl.is_client ,  
  @is_agent=pdl.is_agent ,  
  @is_office=pdl.is_office ,  
  @production_order=pdl.production_order ,  
  @spool_document=pdl.spool_document,  
     @is_message  =  pdl.is_message  
  FROM PMB_Doc_Link pdl  
  JOIN insurance_file ifi  
  ON ifi.product_id = pdl.product_id  
  JOIN process_type pt  
  ON pt.process_type_id = pdl.process_type_id  
  JOIN document_template dt  
  ON dt.Document_Template_Id=pdl.Document_Template_Id  
  WHERE pdl.functional_area = @functional_area  
  AND pdl.process_types_docs_id = @process_types_docs_id  
  AND pt.code = @process_type_code  
 AND ifi.insurance_file_cnt = @insurance_file_cnt  
  AND ifi.source_id = ISNULL(pdl.source_id,ifi.source_id)  
      and dt.code=(select code from #tempcode where id=@cntCode)  
Insert Into #tempDocuments  
SELECT  distinct  
  DT.Document_Template_Id,  
  pdl.Document_Type_Id,  
   @is_client 'is_client',  
  @is_agent 'is_agent',  
  @is_office 'is_office',  
  @production_order 'production_order',  
  @lead_agent_cnt 'lead_agent_cnt' ,  
  dt.description,  
  @spool_document 'spool_document',  
  dt.is_editable_after_merging,  
  @process_types_docs ,  
  @is_message 'is_message',
  dt.email_sub_template_code ,
  dt.email_attachment_template_code, 
  @CORRESPONDENCE_TYPE,
  @DEFAULT_PREFERRED_CORRESPONDENCE,
  @IS_AGENT_RECEIVE_CORRESPONDENCE,
  dt.Code
FROM PMB_Doc_Link pdl  
 JOIN insurance_file ifi  
  ON ifi.product_id = pdl.product_id  
 JOIN process_type pt  
  ON pt.process_type_id = pdl.process_type_id  
JOIn document_type DTs on DTS.document_type_id=pdl.document_type_id  
Join document_template dt On DT.document_type_id=DTS.document_type_id  
WHERE pdl.functional_area = @functional_area  
AND pdl.process_types_docs_id =@process_types_docs_id  
AND pt.code =@process_type_code  
AND ifi.insurance_file_cnt = @insurance_file_cnt and dt.code=( select code from #tempcode where id=@cntCode)  
AND ifi.source_id = ISNULL(pdl.source_id,ifi.source_id) 
AND DT.Document_Template_id IN  
  (SELECT document_template_id  
  FROM document_template dtmp  
  WHERE dtmp.effective_date =  
  (SELECT MAX(effective_date)  
       FROM document_template  
       WHERE document_template.code = ( select code from #tempcode where id=@cntCode)  
        AND CONVERT(DATETIME,(CONVERT(VARCHAR,document_template.effective_date,106))) <= @effective_date  
        AND is_deleted=0  
   )  
    AND is_deleted=0  
)
AND ((@called_from_SAM = 0 AND pdl.generate_through_BO = 1)  
OR  (@called_from_SAM = 1 AND pdl.generate_through_SAM = 1))  

select @cntCode=@cntCode-1  
END  
select * from #tempDocuments  
Drop table #tempCode  
Drop table #tempDocuments  
END  
 
 GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO    
