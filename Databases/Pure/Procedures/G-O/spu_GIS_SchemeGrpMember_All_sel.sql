SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_SchemeGrpMember_All_sel'
GO


CREATE PROCEDURE spu_GIS_SchemeGrpMember_All_sel
AS


SELECT sg.gis_scheme_group_id,
           'Scheme Group Name' = sg.description,
    sg.gis_business_type_id,
           'Business Type Name' = bt.description,
           ISNULL(sgm.gis_scheme_id,0) as gis_scheme_id
      FROM gis_scheme_group sg LEFT JOIN
           gis_scheme_group_member sgm ON 
           (sg.gis_scheme_group_id = sgm.gis_scheme_group_id)
           INNER JOIN gis_business_type bt ON 
           (sg.gis_business_type_id = bt.gis_business_type_id)
     WHERE sg.is_deleted = 0
  ORDER BY sg.description
GO


