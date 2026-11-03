function Pagination(totalrecord, currentindex, pagesize, div, Type, divInfoId, divPagesId) {
    let pageName = '';
    if (divPagesId != '')
        pageName = '_' + divPagesId;


    if (divInfoId == '')
        divInfoId = 'div-pageinfo';
    else
        divInfoId = 'div-pageinfo_' + divInfoId;

    if (divPagesId == '')
        divPagesId = 'div-pages';
    else {
        divPagesId = 'div-pages_' + divPagesId;
        //pageName = '_' + divPagesId;
    }


    $("#" + divInfoId).html('');
    currentindex = parseInt(currentindex);
    totalrecord = parseInt(totalrecord);
    pagesize = parseInt(pagesize);
    var pagestring = '';
    $(div).html("");


    var pagerlink = Math.ceil(totalrecord / pagesize);
    var lastindex = pagerlink - 1;
    if (totalrecord === 0) {
        $("#" + divInfoId).append('<p>Displaying 0 out of 0 items </p>');
    }
    else if (totalrecord > 0) {
        if (currentindex === lastindex) {
            if (currentindex === 0) {
                $("#" + divInfoId).append('<p>Displaying ' + 1 + ' to ' + totalrecord + ' out of ' + totalrecord + ' items </p>');
            }
            else {
                $("#" + divInfoId).append('<p>Displaying ' + parseInt(1 + (pagesize * (currentindex - 1) + parseInt(pagesize))) + ' to ' + totalrecord + ' out of ' + totalrecord + ' items </p>')
            }
        }
        else {
            $("#" + divInfoId).append('<p>Displaying ' + parseInt(pagesize * currentindex + 1) + ' to ' + parseInt(pagesize * currentindex + parseInt(pagesize)) + ' out of ' + totalrecord + ' items </p>')
        }
        $("#" + divInfoId).append('');
        if (totalrecord === 0) {
            pagestring = pagestring + '<a class="paginate_button previous disabled" aria-controls="basic-1" data-dt-idx="0" tabindex="0" id="basic-1_previous">Previous</a>' +
                '<a class="paginate_button previous disabled">Next</a>' +
                '<a class="paginate_button disabled">Last</a>';
        }
        else {
            if (currentindex === 0) {
                pagestring = pagestring +
                    '<a class="paginate_button previous disabled">First</a>' +
                    '<a class="paginate_button previous disabled">Previous</a>';
            }
            else {
                pagestring = pagestring + '<a class="paginate_button" onclick="Search' + pageName + '(0);">First</a>' +
                    '<a class="paginate_button previous" onclick="Search' + pageName + '(' + parseInt(currentindex - 1) + ');">Previous</a>';
            }
            var counter = 0;
            var intial = 0;
            if (parseInt(currentindex) < 5) {
                intial = 0;
            }
            else {
                intial = parseInt(currentindex) - 3;
            }
            pagestring += '<span>';

            for (var i = intial; i < pagerlink; i++) {
                var j = i + 1;
                if (i === currentindex) {
                    pagestring = pagestring + '<a class="paginate_button activebutton hclass" value="' + j + '">' + j + '</a>';
                }
                else {
                    pagestring = pagestring + '<a class="paginate_button hclass" onclick="Search' + pageName + '(' + i + ');" value="' + j + '">' + j + '</a>';
                }
                if (counter === 5)
                    break;
                counter++;
            }

            if (currentindex === lastindex) {
                pagestring = pagestring + '<a class="paginate_button disabled">Next</a>' +
                    '<a class="paginate_button disabled">Last</a>';
            }
            else {
                var nextindex = (parseInt(currentindex) + 1);
                pagestring = pagestring + '<a class="paginate_button hclass" onclick="Search' + pageName + '(' + nextindex + ');">Next</a>' +
                    '<a class="paginate_button" onclick="Search' + pageName + '(' + lastindex + ');">Last</a>';
            }
        }

        pagestring += '</span>';
        btnhideLoader("btnsearch" + pageName);
        $("#" + divPagesId).html('');
        $("#" + divPagesId).append(pagestring);
    }
}


function isChecked(val) {
    if (val)
        return 'checked=""';
    else
        return '';
}


/*allowOnlyArabicAndNumbers*/
function allowOnlyArabic(event) {
    const arabicAndNumbersRegex = /[\u0600-\u06FF0-9-]/;
    const charCode = event.which || event.keyCode;
    const char = String.fromCharCode(charCode);
    if (!arabicAndNumbersRegex.test(char)) {
        event.preventDefault();
        return false;
    }
    return true;
}


function allowOnlyNumbersDot(event) {
    const numbersDotRegex = /[0-9\.]/;
    const charCode = event.which || event.keyCode;
    const char = String.fromCharCode(charCode);
    if (!numbersDotRegex.test(char)) {
        event.preventDefault();
        return false;
    }
    if ((event.which != 46 || $(event.target).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
    return true;
}


var dwindow;
function showPopupWindow(url, width, height) {
    if (dwindow) {
        dwindow.close();
    }
    dwindow = window.open(url, "", "resizable=no,scrollbars=1,top=40,left=150,width=" + width + ",height=" + height);
}