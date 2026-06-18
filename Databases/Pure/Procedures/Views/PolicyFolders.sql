set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'PolicyFolders'
go
-- OBSOLETE BACKWARD COMPATIBILITY VIEW. DO NOT USE THIS IN ANY NEW QUERIES.
create view PolicyFolders as select
    Insurance_Folder.insurance_folder_cnt   ID,
    Insurance_Folder.insurance_holder_cnt   ClientID,
    Source.code                             BranchCode,
    Source.description                      BranchName,
    Insurance_Folder.inception_date         InceptionDate,
    Insurance_Folder.renewal_count          TimesRenewed
    from Insurance_Folder
    left outer join Source on Insurance_Folder.source_id = Source.source_id
go
