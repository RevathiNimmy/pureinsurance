SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_treaty_party_del'
GO


CREATE PROCEDURE spu_treaty_party_del
    @treaty_party_id int = null,
    @treaty_id int = null,
	@UserId int,  
	@UniqueId varchar(50),  
	@ScreenHierarchy varchar(500)
AS
 
    IF @treaty_party_id IS NOT NULL  
    BEGIN   
        DELETE FROM Treaty_party  
        WHERE treaty_party_id = @treaty_party_id;  
    END  
  
    IF @treaty_id IS NOT NULL  
    BEGIN  
       UPDATE tp  
                SET ScreenHierarchy = CONCAT(@ScreenHierarchy, '/Treaty Party(', LTRIM(RTRIM(p.resolved_name)), ')'),  
                    UniqueId = @UniqueId,  
					UserId = @UserId  
                from Treaty_Party tp INNER JOIN Party p ON tp.party_cnt = p.party_cnt
                WHERE treaty_id = @treaty_id; 
				
		DELETE FROM Treaty_party  
        WHERE treaty_id = @treaty_id; 
    END

GO