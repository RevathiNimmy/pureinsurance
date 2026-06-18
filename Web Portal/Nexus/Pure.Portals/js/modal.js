//on page load set controls to lauch modal
$(document).ready(function () {

    init_modal('a.show_modal, a[data = modal]'); //initialise hyperlinks with show_modal class (and thickbox for backwards compatability), also do thickbox for now
    tb_init('a.thickbox, area.thickbox, input.thickbox, input.inlineModal'); //pass where to apply thickbox
    //debugger;
    if (document.URL.indexOf('modal=true') != -1) {
        //set the title from the H1 and remove the H1 
        var sTitle = $('h1').text();
        if (sTitle == "") {
            sTitle = $('h1').text();
        }
        $('.card-heading:first').hide();
        if (self.parent.thisModal != undefined) {
            self.parent.thisModal.setTitle(sTitle.replace(/^\s+|\s+$/g, ''));
        }
    }
    else {
        // var sTitle = $('.card-heading h1').text();
        //$('.card-heading').hide();
        // $('#mstPageTitle').html(sTitle.replace(/^\s+|\s+$/g, ''));
    }
});
var thisModal;
var horizontalPadding = 30;
var verticalPadding = 30;

function show_modal(sUrl, sParams, bInline) {
    if (bInline == false && sUrl.includes("OpenTXTextControl.aspx")) {
        self.thisModal = new BootstrapDialog({
            title: 'Loading...',
            message: $('<iframe src="' + sUrl + '?' + sParams + '"name="TB_iframeContent" class="embed-responsive-item" width="100%" height="820px" scrolling="auto" frameborder="0"></iframe>'),
            size: BootstrapDialog.SIZE_WIDE,
            closeByBackdrop: false,
            closeByKeyboard: false,
            draggable: true
        });
        self.thisModal.open();
    } else if (bInline == false) {
        var iframeHeight = '550px';
        var dialogSize = BootstrapDialog.SIZE_WIDE;
        if (sParams) {
            var paramHeight = sParams.match(/(?:^|&)height=(\d+)/);
            var paramWidth = sParams.match(/(?:^|&)width=(\d+)/);
            if (paramHeight) { iframeHeight = paramHeight[1] + 'px'; }
            if (paramWidth) {
                var w = parseInt(paramWidth[1]);
                if (w <= 600) { dialogSize = BootstrapDialog.SIZE_NORMAL; }
                else if (w <= 900) { dialogSize = BootstrapDialog.SIZE_WIDE; }
            }
        }
        self.thisModal = new BootstrapDialog({
            title: 'Loading...',
            message: $('<iframe src="' + sUrl + '?' + sParams + '"name="TB_iframeContent" class="embed-responsive-item" width="100%" height="' + iframeHeight + '" scrolling="auto" frameborder="0"></iframe>'),
            size: dialogSize,
            closeByBackdrop: false,
            closeByKeyboard: false,
            draggable: true
        });
        self.thisModal.open();
    }
    else {
        //self.thisModal = new BootstrapDialog({
        //    title: $(sUrl).title,
        //    //message:"<input type='button' onlclick='alert('''hurrah''');'/>",
        //    //message: $(sUrl).html(),
        //    size: BootstrapDialog.SIZE_NORMAL,
        //    closeByBackdrop: false,
        //    closeByKeyboard: false,
        //    draggable: true
        //});
        //self.thisModal.open();

        $(sUrl).modal()
    }
}
function resizeParent() {

    //if the parent page is a modal then resize it to fit
    if (document.URL.indexOf('modal=true') != -1) {
        //self.parent.resizeDialog($('.page-container'), window.parent.document);
        self.parent.resizeParentDialog($('.page-container'), window.parent.document);
    }
}

function hide_modal() { //closes the modal
    if ($(thisModal)!=null && $(thisModal)[0] != null && $(thisModal)[0] != 'undefined') {
        $(thisModal)[0].close();
    }
}

function modal_updated(postbacktarget, postbackargument) { //closes the modal, triggering a postback
    hide_modal();
    __doPostBack(postbacktarget, postbackargument);
}

function init_modal(domChunk) {

    //call show modal on the click of control
    $(domChunk).click(function (e) {
        //var sUrl = this.href || this.alt; 0

        //var sUrl = this.Attr("modalURL");
        var sUrl = this.href;
        show_modal(sUrl, null, sUrl.substring(0, 1) == '#' ? true : false, e);
        e.preventDefault();
        //this.blur();
        return false;
    });
}
//below is specific to this modal type, sets the title using the contents of the h1, called from the modal
function setTitle(sTitle) {
    $(thisModal).dialog('option', 'title', sTitle);
}
function tb_init(domChunk) {
    $(domChunk).click(function (event) {

        var t = this.title || this.name || null;
        var a = this.href || this.alt; 0
        var g = this.rel || false;
        tb_show(t, a, g);
        event.preventDefault();
        //this.blur();
        return false;
    });
}

function tb_remove() {
    hide_modal();
}
function tb_show(sIgnore, sUrl, uIgnore2) {
    //split the sUrl to get the params
    var bInline;
    sOutUrl = sUrl.split('?');
    if (sOutUrl[0].indexOf('#TB_inline') != -1) { //inline
        //we need to get the inlineId from the query string
        var sParamSplit = sOutUrl[1].split('&');
        var sParam;
        for (var i = sParamSplit.length - 1; i >= 0; --i) {
            sParam = sParamSplit[i].split('=');
            if (sParam[0] == 'inlineId') { sUrl = '#' + sParam[1]; }
            break;
        }
        bInline = true;
    }
    else { bInline = false; sUrl = sOutUrl[0] }
    show_modal(sUrl, sOutUrl[1], bInline); //pass newmodal=true so that the masterpage knows to load up the jquery ui pieces
}

//this function is called from backend till then page modal is not loaded hence added .5 sec delay.
function ShowPolicyVersionModal(url) {
    setTimeout(function () {
        tb_show(null, url, null);
    }, 250);
}

function tb_updated(postbacktarget, postbackargument) { //added for legacy support
    modal_updated(postbacktarget, postbackargument);
}

$(document).ready(function () {
    $("#linkClientInfo").click(function () {
        if ($("#ctl00_SideInfo_ClientInfo").hasClass("active"))
            $("#ctl00_SideInfo_ClientInfo").removeClass("active");
        else
            $("#ctl00_SideInfo_ClientInfo").addClass("active");
    });

    $("#linkClaimInfo").click(function () {
        if ($("#ctl00_SideInfo_ClaimInfo").hasClass("active"))
            $("#ctl00_SideInfo_ClaimInfo").removeClass("active");
        else
            $("#ctl00_SideInfo_ClaimInfo").addClass("active");
    });
});
