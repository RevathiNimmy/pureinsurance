SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_general_ledger_group_sel'
GO

CREATE PROCEDURE spu_ACT_general_ledger_group_sel
    @document_id_from INT,
    @document_id_to INT
AS
    select s.code as company, 
       convert(datetime, convert(char(10), d.created_date,120), 120) as posting_date,
       b.code as cost_centre, 
       gl.description, 
       dt.code as journal_leg_type, 
       sum(t.amount) amount
from document d,
     documenttype dt,
     transdetail t,
     account a,
     source s,
     sub_branch b,
     structuretree st,
     general_ledger_group gl
where d.documenttype_id = dt.documenttype_id
and d.company_id = s.source_id
and d.sub_branch_id = b.sub_branch_id
and t.document_id = d.document_id
and t.account_id = a.account_id
and st.account_id = a.account_id
and st.parent_node_id = gl.child_structure_tree_id
and d.document_id >= @document_id_from 
and d.document_id <= @document_id_to
group by convert(datetime, convert(char(10), d.created_date,120), 120),s.code, b.code, gl.description, dt.code

GO

