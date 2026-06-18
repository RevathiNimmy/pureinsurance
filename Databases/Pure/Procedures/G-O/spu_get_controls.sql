SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_get_controls
GO

CREATE PROCEDURE spu_get_controls
                @perilid INT,  
                @claimid NUMERIC  
AS  
BEGIN

DECLARE @caption VARCHAR(50),@type INT,@value VARCHAR(100)  
DECLARE @displayorder INT,@mandatory BOOLEAN,@readonly BOOLEAN  
DECLARE @claimpartytypeid INT,@claimlookupid INT  
DECLARE @userdefinedperildataid INT,@perildatadefnid INT,@periltypeid INT  
DECLARE @AgentUnderwriter VARCHAR(1)  
SELECT  @AgentUnderwriter = value  
FROM    hidden_options  
WHERE   branch_id = 1 
    AND option_number = 1  

IF @AgentUnderwriter IS NULL
    SELECT @AgentUnderwriter = 'A'  
IF @AgentUnderwriter = ''  
    SELECT @AgentUnderwriter = 'A'  
  
SELECT @periltypeid=peril_type_id 
FROM   claim_peril 
WHERE  claim_peril_id=@perilid  
  
IF @AgentUnderwriter = 'A'  
BEGIN  
    DECLARE controls_cursor CURSOR FOR  
    SELECT
        user_defined_peril_data.user_defined_peril_data_id,
        peril_data_definition.caption,
        peril_data_definition.type,
        peril_data_definition.display_order,
        peril_data_definition.mandatory,
        peril_data_definition.read_only,
        peril_data_definition.claim_party_type_id,
        peril_data_definition.claim_lookup_id,
        user_defined_peril_data.value,
        peril_data_definition.peril_data_defn_id
    FROM 
        Peril_data_definition
        LEFT OUTER JOIN user_defined_peril_data  
            ON user_defined_peril_data.peril_data_defn_id = peril_data_definition.peril_data_defn_id 
            AND  user_defined_peril_data.claim_id = @claimid   
    WHERE peril_type_id=@periltypeid  
    ORDER BY peril_data_definition.display_order

    OPEN controls_cursor  
    FETCH NEXT FROM controls_cursor  
    INTO @userdefinedperildataid,@caption,@type,@displayorder,@mandatory,  
         @readonly,@claimpartytypeid,  
         @claimlookupid,@value,@perildatadefnid  

    WHILE @@FETCH_STATUS = 0  
    BEGIN  
        IF @userdefinedperildataid IS NULL
        BEGIN  
            IF @type != 6  
                EXEC spu_add_controls @claimid,@perildatadefnid  
        END  
        FETCH NEXT FROM controls_cursor  
         INTO @userdefinedperildataid,@caption,@type,@displayorder,@mandatory,  
              @readonly,@claimpartytypeid,  
              @claimlookupid,@value,@perildatadefnid  
    END  
    CLOSE controls_cursor  
    DEALLOCATE controls_cursor  
    SELECT  
        user_defined_peril_data.user_defined_peril_data_id,  
        peril_data_definition.caption,  
        peril_data_definition.type,  
        peril_data_definition.display_order,  
        peril_data_definition.mandatory,  
        peril_data_definition.read_only,  
        peril_data_definition.claim_party_type_id,  
        peril_data_definition.claim_lookup_id,  
        user_defined_peril_data.value,  
        peril_data_definition.peril_data_defn_id,  
        peril_data_definition.description,  
        -1,  
        'General',  
        TabCount =  1  
    FROM 
        Peril_data_definition
        LEFT OUTER JOIN user_defined_peril_data  
            ON user_defined_peril_data.peril_data_defn_id =peril_data_definition.peril_data_defn_id 
            AND user_defined_peril_data.claim_id = @claimid  
    WHERE   peril_type_id=@periltypeid
    ORDER BY peril_data_definition.Display_Order
END
  
ELSE  
  
BEGIN  
    DECLARE controls_cursor CURSOR FOR  
    SELECT  
        user_defined_peril_data.user_defined_peril_data_id,  
        peril_data_definition.caption,  
        peril_data_definition.type,  
        peril_data_definition.display_order,  
        peril_data_definition.mandatory,  
        peril_data_definition.read_only,  
        peril_data_definition.claim_party_type_id,  
        peril_data_definition.claim_lookup_id,  
        user_defined_peril_data.value,  
        peril_data_definition.peril_data_defn_id  
    FROM 
        Peril_data_definition
        LEFT OUTER JOIN user_defined_peril_data  
            ON user_defined_peril_data.peril_data_defn_id = peril_data_definition.peril_data_defn_id
            AND user_defined_peril_data.claim_id = @claimid    
    WHERE peril_type_id=@periltypeid
    ORDER BY peril_data_definition.display_order

    OPEN controls_cursor  
    FETCH NEXT FROM controls_cursor  
     INTO @userdefinedperildataid,@caption,@type,@displayorder,@mandatory,
          @readonly,@claimpartytypeid,  
          @claimlookupid,@value,@perildatadefnid  

    WHILE @@FETCH_STATUS = 0  
    BEGIN  
        IF @userdefinedperildataid IS NULL
        BEGIN  
            If @type != 6  
             EXEC spu_add_controls @claimid,@perildatadefnid  
        END  
        FETCH NEXT FROM controls_cursor  
        INTO @userdefinedperildataid,@caption,@type,@displayorder,@mandatory,  
             @readonly,@claimpartytypeid,  
             @claimlookupid,@value,@perildatadefnid  
    END
  
    CLOSE controls_cursor  
    DEALLOCATE controls_cursor  
    SELECT
        user_defined_peril_data.user_defined_peril_data_id,  
        peril_data_definition.caption,  
        peril_data_definition.type,  
        peril_data_definition.display_order,  
        peril_data_definition.mandatory,  
        peril_data_definition.read_only,  
        peril_data_definition.claim_party_type_id,  
        peril_data_definition.claim_lookup_id,  
        user_defined_peril_data.value,  
        peril_data_definition.peril_data_defn_id,  
        peril_data_definition.description,  
        ISNULL(Claim_Tab.Claim_Tab_ID,-1),  
        ISNULL(Claim_Tab.Caption,'General'),  
        TabCount =  (SELECT COUNT (DISTINCT Tab_ID)  
                     FROM Peril_data_definition  
                     WHERE peril_type_id=@periltypeid)  
    FROM Peril_data_definition
        LEFT OUTER JOIN user_defined_peril_data
            ON user_defined_peril_data.peril_data_defn_id = peril_data_definition.peril_data_defn_id
            AND user_defined_peril_data.claim_id = @claimid   
        LEFT OUTER JOIN Claim_Tab  
            ON Claim_Tab.Claim_Tab_ID = Peril_data_definition.Tab_ID      
    WHERE   peril_type_id=@periltypeid  
    ORDER BY Claim_Tab.display_order,peril_data_definition.Display_Order
END  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO