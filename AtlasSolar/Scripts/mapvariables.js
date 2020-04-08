// variables
var longitude; // X
var latitude;   // Y
var hdms;       // hemisphere, degrees, minutes, and seconds
var tool = 'info';  // 'info', 'calcenergy', 'calcarea', 'calcefficiency', 'getmeteodata', 'createspp', 'setpoint'
var dialog_info_first = true;
var dialog_calcpv_first = true;
var dialog_calcpvresult_first = true;
var dialog_calcefficiency_first = true;
var dialog_calcefficiencyresult_first = true;
var dontshow_selectpoint = false; // do not show the point selection window on the map
var role = document.getElementById("Role").innerHTML;
var appliances = [];
var calcefficiencyresultchart = null;
var comparepointsresultchart = null;
var set_point_number = 0;
var min_zoom = 4;
var max_zoom = 10;
var months = ["", "", "", "", "", "", "", "", "", "", "", ""];
months[0] = document.getElementById("month01").innerHTML;
months[1] = document.getElementById("month02").innerHTML;
months[2] = document.getElementById("month03").innerHTML;
months[3] = document.getElementById("month04").innerHTML;
months[4] = document.getElementById("month05").innerHTML;
months[5] = document.getElementById("month06").innerHTML;
months[6] = document.getElementById("month07").innerHTML;
months[7] = document.getElementById("month08").innerHTML;
months[8] = document.getElementById("month09").innerHTML;
months[9] = document.getElementById("month10").innerHTML;
months[10] = document.getElementById("month11").innerHTML;
months[11] = document.getElementById("month12").innerHTML;
// lep
var lep_gid = [],
    lep_name_rs = [],
    lep_vltg = [],
    lep_rng = [],
    lep_type = [],
    lep_dtntr = [],
    lep_elvt = [],
    lep_mtrl = [],
    lep_insttp = [],
    lep_lnght = [],
    lep_rnzon = [];
