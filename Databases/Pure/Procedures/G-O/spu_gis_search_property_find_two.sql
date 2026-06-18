SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_gis_search_property_find_two'
GO
CREATE PROCEDURE spu_gis_search_property_find_two
    @gis_data_model_code VARCHAR(10),
    @search_object_name VARCHAR(70) = NULL,
    @search_value VARCHAR(255),
    @is_insurance_ref_reqd tinyint = NULL,
    @Specials_Type_Filter INT =0,
    @Find_Mode INT=0,
    @agent_group_cnt INT =0,
    @User_Id INT=0  ,
    @File_Type int,
    @RetrieveAssociates TINYINT=0
AS
BEGIN
/********************************************************************************************************/
/* Stored Procedure spu_gis_search_property_find, Finds the GIS_Policy_link by doing selects on the      */
/*                  Properties that are marked as search properties within the Data Model.              */
/********************************************************************************************************/
/* Revision             Description of Modification                                     Date        Who */
/* --------             ---------------------------                                     ----        --- */
/* 1.0                  Original                            05/10/2000  RFC */
/* 1.1                  insurance_file_cnt on policy binder holds the folder cnt    12/04/2001  Tom */
/* 1.2                  Get all the details and rename the procedure            26/06/2001  Tom */
/********************************************************************************************************/
SET NOCOUNT ON

DECLARE @SQL                VARCHAR(500) ,
    @SQL2           VARCHAR(500) ,
    @SQL3           VARCHAR(500) ,
    @print              VARCHAR(255) ,
    @gis_data_model_id  INTEGER ,
    @object_name        VARCHAR(70) ,
    @property_name      VARCHAR(70) ,
    @table_name     VARCHAR(70) ,
    @column_name        VARCHAR(70) ,
    @policy_binder_id       INTEGER,
    @specials_type VARCHAR(10),
    @CheckIfUserIsAgent Int ,
    @insurance_file_type_id int,
    @SQLFileType VARCHAR (50)

 if @File_Type  =114
  set @insurance_file_type_id= 1
 if  @File_Type =115
  set @insurance_file_type_id=4
 if  @File_Type =116
  set @insurance_file_type_id=2
 if  @File_Type =117
  set @insurance_file_type_id=3
 if  @File_Type =129
  set @insurance_file_type_id=10
 if  @File_Type =113
  set @insurance_file_type_id=null

    Select @CheckIfUserIsAgent=party_cnt from PMUser Where user_id = @User_id

    If  @CheckIfUserIsAgent IS NULL
    BEGIN
 SET @User_Id = 0
    END

    CREATE TABLE #Matches_Found (policy_binder_id int, object_name varchar(70), property_name varchar(70), value varchar(255),Specials_Type Varchar(10))

    EXEC DDLAddIndex '#Matches_Found','policy_binder_id'

    /* First Select the Data Model ID from the Data Model Code */
    SELECT @gis_data_model_id = gis_data_model_id
    FROM    gis_data_model
    WHERE   code = @gis_data_model_code

    /* Then Build a Cursor for the Search Properties for this Data Model */

    /* If the Object to Search is known then Limit to that Object */

    IF (@search_object_name IS NULL) OR (@search_object_name = "")

        DECLARE c_search_properties CURSOR FAST_FORWARD FOR
        SELECT  object_name,
            table_name,
            property_name,
                column_name,
  specials_type

        FROM    gis_object o
                INNER JOIN gis_property p ON (o.gis_object_id = p.gis_object_id)
        WHERE   o.gis_data_model_id = @gis_data_model_id
 AND
  (
   (@Specials_Type_Filter = 0 AND is_search_property = 1)
   OR
   (@Specials_Type_Filter <> 0)
  )
   ELSE
        DECLARE c_search_properties CURSOR FAST_FORWARD FOR
        SELECT  object_name,
            table_name,
                        property_name,
                column_name,
  specials_type
        FROM    gis_object o
                INNER JOIN gis_property p ON (o.gis_object_id = p.gis_object_id)
        WHERE   is_search_property = 1
         AND   o.gis_data_model_id = @gis_data_model_id
          AND   o.object_name = @search_object_name

    /* Then Loop Round the Cursor and Do the Searches */
    OPEN c_search_properties

    FETCH NEXT FROM c_search_properties
    INTO        @object_name,
            @table_name ,
            @property_name,
            @column_name,
     @specials_type

    WHILE (@@FETCH_STATUS = 0)
    BEGIN
      IF EXISTS(select sysobjects.name from sysobjects  inner join syscolumns on syscolumns.id = sysobjects.id and sysobjects.name = @table_name and syscolumns.name = @column_name)
      BEGIN
        SELECT @SQL = "INSERT INTO #Matches_Found (Policy_Binder_id, object_name, property_name, value, Specials_Type) SELECT " + @gis_data_model_code + "_policy_binder_id, '" + @object_name + "' , '" + @property_name + "' ," + @column_name  + ", '" + @Specials_Type + "'"
        SELECT @SQL2 = " FROM " + @table_name + " WHERE " + @column_name

        --IF RIGHT(@search_value,1) = "%"
		IF CHARINDEX('%', @search_value)>0
            SELECT @SQL2 = @SQL2 + " LIKE "
        ELSE
            IF @specials_type <> 0
                SELECT @SQL2 = @SQL2 + " LIKE "
            ELSE
        SELECT @SQL2 = @SQL2 + " = "

        SELECT @SQL3 = "'" + @search_value + "'"

        EXEC (@SQL + @SQL2 + @SQL3)

      END
        FETCH NEXT FROM c_search_properties
        INTO    @object_name,
                @table_name ,
                @property_name,
                @column_name,
                @specials_type
    END

    CLOSE c_search_properties
    DEALLOCATE c_search_properties

IF @Find_Mode=1
Begin
 SELECT Distinct ifi.insurance_file_id,
  ifi.source_id ins_file_source_id,
  ifi.insurance_file_cnt,
  ifi.insurance_ref,
  ifo.description insurance_folder_code,
  ift.code type_code,
  p.resolved_name insured_name,
  p.shortname insured_shortname,
  p.party_id,
  p.source_id party_source_id,
  ifs.last_modified,
  ifo.insurance_holder_cnt,
  ifo.insurance_folder_cnt,
  ifi.product_id,
  pr.code,
  pr.description caption,
  ifi.lead_agent_cnt,
  ifs.date_created,
  CASE 
     WHEN  ifst.description is null THEN  ift.description else ifst.description  
  END AS Object_Name, 
  --ift.description AS Object_Name , 
  m.Property_Name,
  --m.Value,
  ifi.renewal_date,
  ifi.this_premium,
  ifi.policy_type_id,
  CASE 
     WHEN  ifst.description is null THEN  ift.description else ifst.description  
  END AS type_desc, 
  --ift.description AS type_desc,
  (SELECT TOP 1 shortname
   FROM Party P
   WHERE ifi.lead_agent_cnt  = P.party_cnt) Lead_Agent,
  PT.description AS policy_type,
   ift.insurance_file_type_id,
   
   cast((CASE @RetrieveAssociates
         WHEN 1 THEN (SELECT  
                        P.resolved_name +' ('+ AT.description + ')'  as Name 
		                FROM insurance_file_associates Associate
		                INNER JOIN party P ON Associate.party_cnt = P.party_cnt 
                        INNER JOIN Association_Type AT ON Associate.Association_Type_id=AT.Association_Type_id  
                        Where Associate.Insurance_file_cnt=ifi.Insurance_file_cnt And 
                       (CASE  WHEN ISNUll(Associate.Is_Deleted,0) = 1  And ISNull(Associate.date_removed,Dateadd(year,-99,Getdate())) <= GETDATE() THEN 0   ELSE 1      END =1) 
                         FOR XML AUTO, TYPE )
         ELSE ''
         END) As Varchar(Max)) As AssociatedClients,  
	m.Value 
  FROM    Insurance_File ifi inner join Insurance_File_System ifs on ifs.insurance_file_cnt = ifi.insurance_file_cnt
   inner join Insurance_Folder ifo on  ifo.insurance_folder_cnt = ifi.insurance_folder_cnt
   Inner join Insurance_File_Type ift on ift.insurance_file_type_id = ifi.insurance_file_type_id
   inner join Party p on p.party_cnt = ifo.insurance_holder_cnt
   inner join Product pr on pr.product_id = ifi.product_id
   inner join insurance_file_risk_link ifrl on ifrl.insurance_file_cnt = ifi.insurance_file_cnt
   inner join gis_policy_link gpl on ifrl.risk_cnt = gpl.risk_id
   inner join #Matches_Found m on gpl.gis_policy_link_id = m.policy_binder_id
   inner join Policy_Type PT on PT.policy_type_id = ifi.policy_type_id
   left outer join Insurance_File_Status ifst on ifst.insurance_file_status_id=ifi.insurance_file_status_id
   left outer join party_agent pag on (p.agent_cnt=pag.party_cnt or ifi.lead_agent_cnt=pag.party_cnt)
  WHERE
  ifi.insurance_file_cnt in (select Insurance_File_cnt from insurance_file where insurance_folder_cnt=ifi.insurance_folder_cnt)
  AND (ifi.insurance_file_status_id IS NULL
   OR ifi.insurance_file_status_id = 3   --REN hardcoded as 3
   OR ifi.insurance_file_status_id = 2  -- Lapsed as 2
   OR ifi.insurance_file_type_id = 8)    --MTACan hardcoded as 8
  AND ifi.policy_ignore IS NULL
  and (@insurance_file_type_id is null
    OR IFI.insurance_file_type_id=@insurance_file_type_id)
 AND
  (
   (@Specials_Type_Filter = 0)
   OR
   (@Specials_Type_Filter <> 0 AND m.specials_type = @Specials_Type_Filter)
  )
  AND (
 (@User_id =0)
 OR
 (@User_id<>0 AND ifi.Product_id IN (SELECT product_id FROM party_agent_product pap
    INNER JOIN PMUser pmu ON pmu.party_cnt = pap.party_cnt
      Where pmu.user_id = @User_id))
 )
    and (@agent_group_cnt=0 or pag.linked_account_group=@agent_group_cnt)
 order by ifo.insurance_folder_cnt
End
ELSE
Begin
    SELECT distinct ifi.insurance_file_id,
        ifi.source_id ins_file_source_id,
        ifi.insurance_file_cnt,
        ifi.insurance_ref,
        ifo.description insurance_folder_code,
        ift.code type_code,
        p.resolved_name insured_name,
        p.shortname insured_shortname,
        p.party_id,
        p.source_id party_source_id,
        ifs.last_modified,
        ifo.insurance_holder_cnt,
        ifo.insurance_folder_cnt,
        ifi.product_id,
        pr.code,
        pr.description caption,
        ifi.lead_agent_cnt,
        ifs.date_created,
		CASE 
			WHEN  ifst.description is null THEN  ift.description else ifst.description  
	    END AS Object_Name, 
        --ift.description AS Object_Name , 
        m.Property_Name,
        --m.Value,
 ifi.renewal_date,
 ifi.this_premium,
 ifi.policy_type_id,
 CASE 
     WHEN  ifst.description is null THEN  ift.description else ifst.description  
 END AS type_desc, 
 --ift.description AS type_desc,
 (SELECT TOP 1 shortname
  FROM Party P
  WHERE ifi.lead_agent_cnt  = P.party_cnt) Lead_Agent,
 PT.description AS policy_type,
 ift.insurance_file_type_id,
         m.Value,
                   cast((CASE @RetrieveAssociates
                    WHEN 1 THEN (SELECT  
                       Associate.resolved_name Name
		                FROM insurance_file_associates IFA
		                INNER JOIN party Associate ON IFA.party_cnt = Associate.party_cnt  
                        Where Ifa.Insurance_file_cnt=ifi.Insurance_file_cnt And 
                         ISNUll(IFA.Is_Deleted,0) <>1
                         FOR XML AUTO, TYPE )
         ELSE ''
         END) As Varchar(Max)) As AssociatedClients   
    FROM    Insurance_File ifi inner join Insurance_File_System ifs on ifs.insurance_file_cnt = ifi.insurance_file_cnt
        inner join Insurance_Folder ifo on  ifo.insurance_folder_cnt = ifi.insurance_folder_cnt
        Inner join Insurance_File_Type ift on ift.insurance_file_type_id = ifi.insurance_file_type_id
        inner join Party p on p.party_cnt = ifo.insurance_holder_cnt
        inner join Product pr on pr.product_id = ifi.product_id
        inner join insurance_file_risk_link ifrl on ifrl.insurance_file_cnt = ifi.insurance_file_cnt
  inner join gis_policy_link gpl on ifrl.risk_cnt = gpl.risk_id
        inner join #Matches_Found m on gpl.gis_policy_link_id = m.policy_binder_id
  inner join Policy_Type PT on PT.policy_type_id = ifi.policy_type_id
 --PN_71945 Start
  left outer join Insurance_File_Status ifst on ifst.insurance_file_status_id=ifi.insurance_file_status_id
  left outer join party_agent pag on (p.agent_cnt=pag.party_cnt or ifi.lead_agent_cnt=pag.party_cnt)
    WHERE
     ifi.insurance_file_cnt in (select Insurance_File_cnt from insurance_file where insurance_folder_cnt=ifi.insurance_folder_cnt )
    --PN_72945 End
    AND (ifi.insurance_file_status_id IS NULL
        OR ifi.insurance_file_status_id = 3   --REN hardcoded as 3
        OR ifi.insurance_file_status_id = 2   -- Lapsed as 2
        OR ifi.insurance_file_type_id = 8)    --MTACan hardcoded as 8
    AND ifi.policy_ignore IS NULL
 and (@insurance_file_type_id is null
   OR IFI.insurance_file_type_id=@insurance_file_type_id)
 AND
 (
  (@Specials_Type_Filter = 0)
  OR
  (@Specials_Type_Filter <> 0 AND m.specials_type = @Specials_Type_Filter)
    )
    AND (
(@User_id =0)
OR
(@User_id<>0 AND ifi.Product_id IN (SELECT product_id FROM party_agent_product pap
      INNER JOIN PMUser pmu ON pmu.party_cnt = pap.party_cnt
              Where pmu.user_id = @User_id))
)
    and (@agent_group_cnt=0 or pag.linked_account_group=@agent_group_cnt)
    order by ifo.insurance_folder_cnt
End

    DROP TABLE #Matches_Found

SET NOCOUNT OFF

END

GO
