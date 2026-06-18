EXECUTE DDLDropProcedure 'spu_GIS_Policy_Numbers_Get_RegEx_Fields'
GO

CREATE PROCEDURE spu_GIS_Policy_Numbers_Get_RegEx_Fields
    @SchemeID integer,
    @SourceID integer
AS

IF EXISTS (SELECT * FROM GIS_Branch_Scheme
           WHERE gis_scheme_id = @SchemeID
           AND source_id = @SourceID) BEGIN

       SELECT s.scheme_no,
              gsa.activation_level,
              i.code,
              b.agency_code,
              c.pm_company_number,
              c.broker_abi_id,
              c.sender_mailbox_id,
              s.qm_insurer_ref
         FROM GIS_Scheme s
    LEFT JOIN GIS_Insurer i ON s.gis_insurer_id = i.gis_insurer_id
    LEFT JOIN GIS_Branch_Scheme b ON s.gis_scheme_id = b.gis_scheme_id
    LEFT JOIN Source c ON b.source_id = c.source_id
    LEFT OUTER JOIN gis_scheme_activation gsa ON 
                        s.class_of_business = gsa.class_of_business 
                        and s.qm_insurer_ref = gsa.qm_insurer_ref 
                        and s.scheme_no = gsa.scheme_no 
                        and c.sender_mailbox_id = gsa.mailbox 
                        and c.is_deleted = 0
        WHERE s.gis_scheme_id = @SchemeID
          AND b.source_id = @SourceID

END ELSE BEGIN

       SELECT s.scheme_no,
	          s.activation_level,
	          i.code,
	          '' AS agency_code,
	          @SourceID AS source_id,
	          c.broker_abi_id,
	          c.sender_mailbox_id,
                  s.qm_insurer_ref
	     FROM GIS_Scheme s
    LEFT JOIN GIS_Insurer i ON s.gis_insurer_id = i.gis_insurer_id
    LEFT JOIN Source c ON c.source_id = @SourceID
        WHERE s.gis_scheme_id = @SchemeID

END

