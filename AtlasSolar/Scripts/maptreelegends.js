// layer tree legends
$(function () {
    $.jstree.defaults.core.themes.icons = false;
    $('#jstree').jstree({
        core: {
            check_callback: false
        },
        checkbox: {
            keep_selected_style: false,
            three_state: false, // to avoid that fact that checking a node also check others
            whole_node: false,  // to avoid checking the box just clicking the node
            tie_selection: false // for checking without selecting and selecting without checking
        },
        plugins: ['checkbox']
    })
        .on("check_node.jstree uncheck_node.jstree open_node.jstree", function (e, data) {
        if (data.node.id == 'spp') {
            Layer_spp.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'oblasti') {
            Layer_oblasti.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'lep') {
            Layer_lep.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'lep_bufer') {
            Layer_lep_bufer.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'lep2') {
            Layer_lep2.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'arheopamyat') {
            Layer_arheopamyat.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'hidroohrzony') {
            Layer_hidroohrzony.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'pamyatnikprirodypol') {
            Layer_pamyatnikprirodypol.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'prirparki') {
            Layer_prirparki.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rezervaty') {
            Layer_rezervaty.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'zakazniky') {
            Layer_zakazniky.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'zapovedniki') {
            Layer_zapovedniki.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'zapovedzony') {
            Layer_zapovedzony.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'srtm') {
            Layer_srtm.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'aspect_srtm') {
            Layer_aspect_srtm.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'slope_srtm') {
            Layer_slope_srtm.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_dnr_year') {
            Layer_avg_dnr_year.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_dnr_01') {
            Layer_avg_dnr_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_dnr_02') {
            Layer_avg_dnr_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_dnr_03') {
            Layer_avg_dnr_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_dnr_04') {
            Layer_avg_dnr_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_dnr_05') {
            Layer_avg_dnr_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_dnr_06') {
            Layer_avg_dnr_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_dnr_07') {
            Layer_avg_dnr_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_dnr_08') {
            Layer_avg_dnr_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_dnr_09') {
            Layer_avg_dnr_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_dnr_10') {
            Layer_avg_dnr_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_dnr_11') {
            Layer_avg_dnr_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_dnr_12') {
            Layer_avg_dnr_12.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'swv_dwn_year') {
            Layer_swv_dwn_year.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'swv_dwn_01') {
            Layer_swv_dwn_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'swv_dwn_02') {
            Layer_swv_dwn_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'swv_dwn_03') {
            Layer_swv_dwn_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'swv_dwn_04') {
            Layer_swv_dwn_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'swv_dwn_05') {
            Layer_swv_dwn_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'swv_dwn_06') {
            Layer_swv_dwn_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'swv_dwn_07') {
            Layer_swv_dwn_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'swv_dwn_08') {
            Layer_swv_dwn_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'swv_dwn_09') {
            Layer_swv_dwn_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'swv_dwn_10') {
            Layer_swv_dwn_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'swv_dwn_11') {
            Layer_swv_dwn_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'swv_dwn_12') {
            Layer_swv_dwn_12.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'exp_dif_year') {
            Layer_exp_dif_year.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'exp_dif_01') {
            Layer_exp_dif_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'exp_dif_02') {
            Layer_exp_dif_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'exp_dif_03') {
            Layer_exp_dif_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'exp_dif_04') {
            Layer_exp_dif_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'exp_dif_05') {
            Layer_exp_dif_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'exp_dif_06') {
            Layer_exp_dif_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'exp_dif_07') {
            Layer_exp_dif_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'exp_dif_08') {
            Layer_exp_dif_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'exp_dif_09') {
            Layer_exp_dif_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'exp_dif_10') {
            Layer_exp_dif_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'exp_dif_11') {
            Layer_exp_dif_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'exp_dif_12') {
            Layer_exp_dif_12.setVisible(data.node.state.checked);
        }

        if (data.node.id == 'rettlt0opt_year') {
            Layer_rettlt0opt_year.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rettlt0opt_01') {
            Layer_rettlt0opt_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rettlt0opt_02') {
            Layer_rettlt0opt_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rettlt0opt_03') {
            Layer_rettlt0opt_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rettlt0opt_04') {
            Layer_rettlt0opt_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rettlt0opt_05') {
            Layer_rettlt0opt_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rettlt0opt_06') {
            Layer_rettlt0opt_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rettlt0opt_07') {
            Layer_rettlt0opt_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rettlt0opt_08') {
            Layer_rettlt0opt_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rettlt0opt_09') {
            Layer_rettlt0opt_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rettlt0opt_10') {
            Layer_rettlt0opt_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rettlt0opt_11') {
            Layer_rettlt0opt_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rettlt0opt_12') {
            Layer_rettlt0opt_12.setVisible(data.node.state.checked);
        }

        if (data.node.id == 'clrskyavrg_year') {
            Layer_clrskyavrg_year.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'clrskyavrg_01') {
            Layer_clrskyavrg_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'clrskyavrg_02') {
            Layer_clrskyavrg_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'clrskyavrg_03') {
            Layer_clrskyavrg_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'clrskyavrg_04') {
            Layer_clrskyavrg_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'clrskyavrg_05') {
            Layer_clrskyavrg_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'clrskyavrg_06') {
            Layer_clrskyavrg_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'clrskyavrg_07') {
            Layer_clrskyavrg_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'clrskyavrg_08') {
            Layer_clrskyavrg_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'clrskyavrg_09') {
            Layer_clrskyavrg_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'clrskyavrg_10') {
            Layer_clrskyavrg_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'clrskyavrg_11') {
            Layer_clrskyavrg_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'clrskyavrg_12') {
            Layer_clrskyavrg_12.setVisible(data.node.state.checked);
        }

        if (data.node.id == 'retesh0mim_year') {
            Layer_retesh0mim_year.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'retesh0mim_01') {
            Layer_retesh0mim_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'retesh0mim_02') {
            Layer_retesh0mim_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'retesh0mim_03') {
            Layer_retesh0mim_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'retesh0mim_04') {
            Layer_retesh0mim_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'retesh0mim_05') {
            Layer_retesh0mim_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'retesh0mim_06') {
            Layer_retesh0mim_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'retesh0mim_07') {
            Layer_retesh0mim_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'retesh0mim_08') {
            Layer_retesh0mim_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'retesh0mim_09') {
            Layer_retesh0mim_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'retesh0mim_10') {
            Layer_retesh0mim_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'retesh0mim_11') {
            Layer_retesh0mim_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'retesh0mim_12') {
            Layer_retesh0mim_12.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_01') {
            Layer_t10m_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_02') {
            Layer_t10m_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_03') {
            Layer_t10m_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_04') {
            Layer_t10m_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_05') {
            Layer_t10m_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_06') {
            Layer_t10m_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_07') {
            Layer_t10m_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_08') {
            Layer_t10m_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_09') {
            Layer_t10m_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_10') {
            Layer_t10m_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_11') {
            Layer_t10m_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_12') {
            Layer_t10m_12.setVisible(data.node.state.checked);
        }

        if (data.node.id == 'rainavgesm_year') {
            Layer_rainavgesm_year.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rainavgesm_01') {
            Layer_rainavgesm_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rainavgesm_02') {
            Layer_rainavgesm_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rainavgesm_03') {
            Layer_rainavgesm_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rainavgesm_04') {
            Layer_rainavgesm_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rainavgesm_05') {
            Layer_rainavgesm_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rainavgesm_06') {
            Layer_rainavgesm_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rainavgesm_07') {
            Layer_rainavgesm_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rainavgesm_08') {
            Layer_rainavgesm_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rainavgesm_09') {
            Layer_rainavgesm_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rainavgesm_10') {
            Layer_rainavgesm_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rainavgesm_11') {
            Layer_rainavgesm_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'rainavgesm_12') {
            Layer_rainavgesm_12.setVisible(data.node.state.checked);
        }

        if (data.node.id == 't10mmax_01') {
            Layer_t10mmax_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10mmax_02') {
            Layer_t10mmax_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10mmax_03') {
            Layer_t10mmax_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10mmax_04') {
            Layer_t10mmax_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10mmax_05') {
            Layer_t10mmax_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10mmax_06') {
            Layer_t10mmax_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10mmax_07') {
            Layer_t10mmax_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10mmax_08') {
            Layer_t10mmax_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10mmax_09') {
            Layer_t10mmax_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10mmax_10') {
            Layer_t10mmax_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10mmax_11') {
            Layer_t10mmax_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10mmax_12') {
            Layer_t10mmax_12.setVisible(data.node.state.checked);
        }

        if (data.node.id == 't10m_min_01') {
            Layer_t10m_min_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_min_02') {
            Layer_t10m_min_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_min_03') {
            Layer_t10m_min_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_min_04') {
            Layer_t10m_min_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_min_05') {
            Layer_t10m_min_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_min_06') {
            Layer_t10m_min_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_min_07') {
            Layer_t10m_min_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_min_08') {
            Layer_t10m_min_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_min_09') {
            Layer_t10m_min_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_min_10') {
            Layer_t10m_min_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_min_11') {
            Layer_t10m_min_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 't10m_min_12') {
            Layer_t10m_min_12.setVisible(data.node.state.checked);
        }

        if (data.node.id == 'tskinavg_01') {
            Layer_tskinavg_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'tskinavg_02') {
            Layer_tskinavg_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'tskinavg_03') {
            Layer_tskinavg_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'tskinavg_04') {
            Layer_tskinavg_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'tskinavg_05') {
            Layer_tskinavg_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'tskinavg_06') {
            Layer_tskinavg_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'tskinavg_07') {
            Layer_tskinavg_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'tskinavg_08') {
            Layer_tskinavg_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'tskinavg_09') {
            Layer_tskinavg_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'tskinavg_10') {
            Layer_tskinavg_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'tskinavg_11') {
            Layer_tskinavg_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'tskinavg_12') {
            Layer_tskinavg_12.setVisible(data.node.state.checked);
        }

        if (data.node.id == 'srfalbavg_01') {
            Layer_srfalbavg_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'srfalbavg_02') {
            Layer_srfalbavg_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'srfalbavg_03') {
            Layer_srfalbavg_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'srfalbavg_04') {
            Layer_srfalbavg_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'srfalbavg_05') {
            Layer_srfalbavg_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'srfalbavg_06') {
            Layer_srfalbavg_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'srfalbavg_07') {
            Layer_srfalbavg_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'srfalbavg_08') {
            Layer_srfalbavg_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'srfalbavg_09') {
            Layer_srfalbavg_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'srfalbavg_10') {
            Layer_srfalbavg_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'srfalbavg_11') {
            Layer_srfalbavg_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'srfalbavg_12') {
            Layer_srfalbavg_12.setVisible(data.node.state.checked);
        }
        
        if (data.node.id == 'avg_kt_22_01') {
            Layer_avg_kt_22_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_kt_22_02') {
            Layer_avg_kt_22_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_kt_22_03') {
            Layer_avg_kt_22_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_kt_22_04') {
            Layer_avg_kt_22_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_kt_22_05') {
            Layer_avg_kt_22_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_kt_22_06') {
            Layer_avg_kt_22_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_kt_22_07') {
            Layer_avg_kt_22_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_kt_22_08') {
            Layer_avg_kt_22_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_kt_22_09') {
            Layer_avg_kt_22_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_kt_22_10') {
            Layer_avg_kt_22_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_kt_22_11') {
            Layer_avg_kt_22_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_kt_22_12') {
            Layer_avg_kt_22_12.setVisible(data.node.state.checked);
        }
        
        if (data.node.id == 'avg_nkt_22_01') {
            Layer_avg_nkt_22_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_nkt_22_02') {
            Layer_avg_nkt_22_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_nkt_22_03') {
            Layer_avg_nkt_22_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_nkt_22_04') {
            Layer_avg_nkt_22_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_nkt_22_05') {
            Layer_avg_nkt_22_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_nkt_22_06') {
            Layer_avg_nkt_22_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_nkt_22_07') {
            Layer_avg_nkt_22_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_nkt_22_08') {
            Layer_avg_nkt_22_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_nkt_22_09') {
            Layer_avg_nkt_22_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_nkt_22_10') {
            Layer_avg_nkt_22_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_nkt_22_11') {
            Layer_avg_nkt_22_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'avg_nkt_22_12') {
            Layer_avg_nkt_22_12.setVisible(data.node.state.checked);
        }
        
        if (data.node.id == 'day_cld_22_01') {
            Layer_day_cld_22_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'day_cld_22_02') {
            Layer_day_cld_22_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'day_cld_22_03') {
            Layer_day_cld_22_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'day_cld_22_04') {
            Layer_day_cld_22_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'day_cld_22_05') {
            Layer_day_cld_22_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'day_cld_22_06') {
            Layer_day_cld_22_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'day_cld_22_07') {
            Layer_day_cld_22_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'day_cld_22_08') {
            Layer_day_cld_22_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'day_cld_22_09') {
            Layer_day_cld_22_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'day_cld_22_10') {
            Layer_day_cld_22_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'day_cld_22_11') {
            Layer_day_cld_22_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'day_cld_22_12') {
            Layer_day_cld_22_12.setVisible(data.node.state.checked);
        }
        
        if (data.node.id == 'daylghtav_01') {
            Layer_daylghtav_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'daylghtav_02') {
            Layer_daylghtav_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'daylghtav_03') {
            Layer_daylghtav_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'daylghtav_04') {
            Layer_daylghtav_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'daylghtav_05') {
            Layer_daylghtav_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'daylghtav_06') {
            Layer_daylghtav_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'daylghtav_07') {
            Layer_daylghtav_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'daylghtav_08') {
            Layer_daylghtav_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'daylghtav_09') {
            Layer_daylghtav_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'daylghtav_10') {
            Layer_daylghtav_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'daylghtav_11') {
            Layer_daylghtav_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'daylghtav_12') {
            Layer_daylghtav_12.setVisible(data.node.state.checked);
        }

        if (data.node.id == 'dni_year') {
            Layer_dni_year.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'dni_01') {
            Layer_dni_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'dni_02') {
            Layer_dni_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'dni_03') {
            Layer_dni_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'dni_04') {
            Layer_dni_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'dni_05') {
            Layer_dni_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'dni_06') {
            Layer_dni_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'dni_07') {
            Layer_dni_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'dni_08') {
            Layer_dni_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'dni_09') {
            Layer_dni_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'dni_10') {
            Layer_dni_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'dni_11') {
            Layer_dni_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'dni_12') {
            Layer_dni_12.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sic_year') {
            Layer_sic_year.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sic_01') {
            Layer_sic_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sic_02') {
            Layer_sic_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sic_03') {
            Layer_sic_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sic_04') {
            Layer_sic_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sic_05') {
            Layer_sic_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sic_06') {
            Layer_sic_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sic_07') {
            Layer_sic_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sic_08') {
            Layer_sic_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sic_09') {
            Layer_sic_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sic_10') {
            Layer_sic_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sic_11') {
            Layer_sic_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sic_12') {
            Layer_sic_12.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sid_year') {
            Layer_sid_year.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sid_01') {
            Layer_sid_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sid_02') {
            Layer_sid_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sid_03') {
            Layer_sid_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sid_04') {
            Layer_sid_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sid_05') {
            Layer_sid_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sid_06') {
            Layer_sid_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sid_07') {
            Layer_sid_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sid_08') {
            Layer_sid_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sid_09') {
            Layer_sid_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sid_10') {
            Layer_sid_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sid_11') {
            Layer_sid_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sid_12') {
            Layer_sid_12.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_year') {
            Layer_sis_year.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_01') {
            Layer_sis_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_02') {
            Layer_sis_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_03') {
            Layer_sis_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_04') {
            Layer_sis_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_05') {
            Layer_sis_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_06') {
            Layer_sis_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_07') {
            Layer_sis_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_08') {
            Layer_sis_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_09') {
            Layer_sis_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_10') {
            Layer_sis_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_11') {
            Layer_sis_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_12') {
            Layer_sis_12.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_klr_year') {
            Layer_sis_klr_year.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_klr_01') {
            Layer_sis_klr_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_klr_02') {
            Layer_sis_klr_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_klr_03') {
            Layer_sis_klr_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_klr_04') {
            Layer_sis_klr_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_klr_05') {
            Layer_sis_klr_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_klr_06') {
            Layer_sis_klr_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_klr_07') {
            Layer_sis_klr_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_klr_08') {
            Layer_sis_klr_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_klr_09') {
            Layer_sis_klr_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_klr_10') {
            Layer_sis_klr_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_klr_11') {
            Layer_sis_klr_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'sis_klr_12') {
            Layer_sis_klr_12.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_clr_cky_year') {
            Layer_p_clr_cky_year.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_clr_cky_01') {
            Layer_p_clr_cky_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_clr_cky_02') {
            Layer_p_clr_cky_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_clr_cky_03') {
            Layer_p_clr_cky_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_clr_cky_04') {
            Layer_p_clr_cky_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_clr_cky_05') {
            Layer_p_clr_cky_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_clr_cky_06') {
            Layer_p_clr_cky_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_clr_cky_07') {
            Layer_p_clr_cky_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_clr_cky_08') {
            Layer_p_clr_cky_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_clr_cky_09') {
            Layer_p_clr_cky_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_clr_cky_10') {
            Layer_p_clr_cky_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_clr_cky_11') {
            Layer_p_clr_cky_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_clr_cky_12') {
            Layer_p_clr_cky_12.setVisible(data.node.state.checked);
        }
        //
        if (data.node.id == 'p_swv_dwn_year') {
            Layer_p_swv_dwn_year.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_swv_dwn_01') {
            Layer_p_swv_dwn_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_swv_dwn_02') {
            Layer_p_swv_dwn_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_swv_dwn_03') {
            Layer_p_swv_dwn_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_swv_dwn_04') {
            Layer_p_swv_dwn_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_swv_dwn_05') {
            Layer_p_swv_dwn_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_swv_dwn_06') {
            Layer_p_swv_dwn_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_swv_dwn_07') {
            Layer_p_swv_dwn_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_swv_dwn_08') {
            Layer_p_swv_dwn_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_swv_dwn_09') {
            Layer_p_swv_dwn_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_swv_dwn_10') {
            Layer_p_swv_dwn_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_swv_dwn_11') {
            Layer_p_swv_dwn_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_swv_dwn_12') {
            Layer_p_swv_dwn_12.setVisible(data.node.state.checked);
        }
        //
        if (data.node.id == 'p_toa_dwn_year') {
            Layer_p_toa_dwn_year.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_toa_dwn_01') {
            Layer_p_toa_dwn_01.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_toa_dwn_02') {
            Layer_p_toa_dwn_02.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_toa_dwn_03') {
            Layer_p_toa_dwn_03.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_toa_dwn_04') {
            Layer_p_toa_dwn_04.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_toa_dwn_05') {
            Layer_p_toa_dwn_05.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_toa_dwn_06') {
            Layer_p_toa_dwn_06.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_toa_dwn_07') {
            Layer_p_toa_dwn_07.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_toa_dwn_08') {
            Layer_p_toa_dwn_08.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_toa_dwn_09') {
            Layer_p_toa_dwn_09.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_toa_dwn_10') {
            Layer_p_toa_dwn_10.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_toa_dwn_11') {
            Layer_p_toa_dwn_11.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'p_toa_dwn_12') {
            Layer_p_toa_dwn_12.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'kzcover') {
            Layer_kzcover.setVisible(data.node.state.checked);
        }
        if (data.node.id == 'meteo_st') {
            Layer_meteo_st.setVisible(data.node.state.checked);
        }
        // select
        var nasasse = false;
        var nasasse1 = false;
        var nasasse2 = false;
        var nasasse3 = false;
        var nasasse4 = false;
        var nasasse5 = false;
        var nasasse6 = false;
        if (Layer_avg_dnr_year.getVisible() == false &&
            Layer_avg_dnr_01.getVisible() == false &&
            Layer_avg_dnr_02.getVisible() == false &&
            Layer_avg_dnr_03.getVisible() == false &&
            Layer_avg_dnr_04.getVisible() == false &&
            Layer_avg_dnr_05.getVisible() == false &&
            Layer_avg_dnr_06.getVisible() == false &&
            Layer_avg_dnr_07.getVisible() == false &&
            Layer_avg_dnr_08.getVisible() == false &&
            Layer_avg_dnr_09.getVisible() == false &&
            Layer_avg_dnr_10.getVisible() == false &&
            Layer_avg_dnr_11.getVisible() == false &&
            Layer_avg_dnr_12.getVisible() == false) {
            $('#j1_7_anchor').css('font-weight', '');
        }
        else {
            $('#j1_7_anchor').css('font-weight', 'bold');
            nasasse1 = true;
        }
        if (Layer_swv_dwn_year.getVisible() == false &&
            Layer_swv_dwn_01.getVisible() == false &&
            Layer_swv_dwn_02.getVisible() == false &&
            Layer_swv_dwn_03.getVisible() == false &&
            Layer_swv_dwn_04.getVisible() == false &&
            Layer_swv_dwn_05.getVisible() == false &&
            Layer_swv_dwn_06.getVisible() == false &&
            Layer_swv_dwn_07.getVisible() == false &&
            Layer_swv_dwn_08.getVisible() == false &&
            Layer_swv_dwn_09.getVisible() == false &&
            Layer_swv_dwn_10.getVisible() == false &&
            Layer_swv_dwn_11.getVisible() == false &&
            Layer_swv_dwn_12.getVisible() == false) {
            $('#j1_21_anchor').css('font-weight', '');
        }
        else {
            $('#j1_21_anchor').css('font-weight', 'bold');
            nasasse2 = true;
        }
        if (Layer_exp_dif_year.getVisible() == false &&
            Layer_exp_dif_01.getVisible() == false &&
            Layer_exp_dif_02.getVisible() == false &&
            Layer_exp_dif_03.getVisible() == false &&
            Layer_exp_dif_04.getVisible() == false &&
            Layer_exp_dif_05.getVisible() == false &&
            Layer_exp_dif_06.getVisible() == false &&
            Layer_exp_dif_07.getVisible() == false &&
            Layer_exp_dif_08.getVisible() == false &&
            Layer_exp_dif_09.getVisible() == false &&
            Layer_exp_dif_10.getVisible() == false &&
            Layer_exp_dif_11.getVisible() == false &&
            Layer_exp_dif_12.getVisible() == false) {
            $('#j1_35_anchor').css('font-weight', '');
        }
        else {
            $('#j1_35_anchor').css('font-weight', 'bold');
            nasasse3 = true;
        }
        if (Layer_rettlt0opt_year.getVisible() == false &&
            Layer_rettlt0opt_01.getVisible() == false &&
            Layer_rettlt0opt_02.getVisible() == false &&
            Layer_rettlt0opt_03.getVisible() == false &&
            Layer_rettlt0opt_04.getVisible() == false &&
            Layer_rettlt0opt_05.getVisible() == false &&
            Layer_rettlt0opt_06.getVisible() == false &&
            Layer_rettlt0opt_07.getVisible() == false &&
            Layer_rettlt0opt_08.getVisible() == false &&
            Layer_rettlt0opt_09.getVisible() == false &&
            Layer_rettlt0opt_10.getVisible() == false &&
            Layer_rettlt0opt_11.getVisible() == false &&
            Layer_rettlt0opt_12.getVisible() == false) {
            $('#j1_49_anchor').css('font-weight', '');
        }
        else {
            $('#j1_49_anchor').css('font-weight', 'bold');
            nasasse4 = true;
        }
        if (Layer_clrskyavrg_year.getVisible() == false &&
            Layer_clrskyavrg_01.getVisible() == false &&
            Layer_clrskyavrg_02.getVisible() == false &&
            Layer_clrskyavrg_03.getVisible() == false &&
            Layer_clrskyavrg_04.getVisible() == false &&
            Layer_clrskyavrg_05.getVisible() == false &&
            Layer_clrskyavrg_06.getVisible() == false &&
            Layer_clrskyavrg_07.getVisible() == false &&
            Layer_clrskyavrg_08.getVisible() == false &&
            Layer_clrskyavrg_09.getVisible() == false &&
            Layer_clrskyavrg_10.getVisible() == false &&
            Layer_clrskyavrg_11.getVisible() == false &&
            Layer_clrskyavrg_12.getVisible() == false) {
            $('#j1_63_anchor').css('font-weight', '');
        }
        else {
            $('#j1_63_anchor').css('font-weight', 'bold');
            nasasse5 = true;
        }
        if (Layer_retesh0mim_year.getVisible() == false &&
            Layer_retesh0mim_01.getVisible() == false &&
            Layer_retesh0mim_02.getVisible() == false &&
            Layer_retesh0mim_03.getVisible() == false &&
            Layer_retesh0mim_04.getVisible() == false &&
            Layer_retesh0mim_05.getVisible() == false &&
            Layer_retesh0mim_06.getVisible() == false &&
            Layer_retesh0mim_07.getVisible() == false &&
            Layer_retesh0mim_08.getVisible() == false &&
            Layer_retesh0mim_09.getVisible() == false &&
            Layer_retesh0mim_10.getVisible() == false &&
            Layer_retesh0mim_11.getVisible() == false &&
            Layer_retesh0mim_12.getVisible() == false) {
            $('#j1_77_anchor').css('font-weight', '');
        }
        else {
            $('#j1_77_anchor').css('font-weight', 'bold');
            nasasse6 = true;
        }
        if (nasasse1 == false &&
            nasasse2 == false &&
            nasasse3 == false &&
            nasasse4 == false &&
            nasasse5 == false &&
            nasasse6 == false) {
            $('#j1_6_anchor').css('font-weight', '');
        }
        else {
            $('#j1_6_anchor').css('font-weight', 'bold');
            nasasse = true;
        }
        var saraeh = false;
        var saraeh1 = false;
        var saraeh2 = false;
        var saraeh3 = false;
        var saraeh4 = false;
        if (Layer_dni_year.getVisible() == false &&
            Layer_dni_01.getVisible() == false &&
            Layer_dni_02.getVisible() == false &&
            Layer_dni_03.getVisible() == false &&
            Layer_dni_04.getVisible() == false &&
            Layer_dni_05.getVisible() == false &&
            Layer_dni_06.getVisible() == false &&
            Layer_dni_07.getVisible() == false &&
            Layer_dni_08.getVisible() == false &&
            Layer_dni_09.getVisible() == false &&
            Layer_dni_10.getVisible() == false &&
            Layer_dni_11.getVisible() == false &&
            Layer_dni_12.getVisible() == false) {
            $('#j1_92_anchor').css('font-weight', '');
        }
        else {
            $('#j1_92_anchor').css('font-weight', 'bold');
            saraeh1 = true;
        }
        if (Layer_sic_year.getVisible() == false &&
            Layer_sic_01.getVisible() == false &&
            Layer_sic_02.getVisible() == false &&
            Layer_sic_03.getVisible() == false &&
            Layer_sic_04.getVisible() == false &&
            Layer_sic_05.getVisible() == false &&
            Layer_sic_06.getVisible() == false &&
            Layer_sic_07.getVisible() == false &&
            Layer_sic_08.getVisible() == false &&
            Layer_sic_09.getVisible() == false &&
            Layer_sic_10.getVisible() == false &&
            Layer_sic_11.getVisible() == false &&
            Layer_sic_12.getVisible() == false) {
            $('#j1_106_anchor').css('font-weight', '');
        }
        else {
            $('#j1_106_anchor').css('font-weight', 'bold');
            saraeh2 = true;
        }
        if (Layer_sid_year.getVisible() == false &&
            Layer_sid_01.getVisible() == false &&
            Layer_sid_02.getVisible() == false &&
            Layer_sid_03.getVisible() == false &&
            Layer_sid_04.getVisible() == false &&
            Layer_sid_05.getVisible() == false &&
            Layer_sid_06.getVisible() == false &&
            Layer_sid_07.getVisible() == false &&
            Layer_sid_08.getVisible() == false &&
            Layer_sid_09.getVisible() == false &&
            Layer_sid_10.getVisible() == false &&
            Layer_sid_11.getVisible() == false &&
            Layer_sid_12.getVisible() == false) {
            $('#j1_120_anchor').css('font-weight', '');
        }
        else {
            $('#j1_120_anchor').css('font-weight', 'bold');
            saraeh3 = true;
        }
        if (Layer_sis_year.getVisible() == false &&
            Layer_sis_01.getVisible() == false &&
            Layer_sis_02.getVisible() == false &&
            Layer_sis_03.getVisible() == false &&
            Layer_sis_04.getVisible() == false &&
            Layer_sis_05.getVisible() == false &&
            Layer_sis_06.getVisible() == false &&
            Layer_sis_07.getVisible() == false &&
            Layer_sis_08.getVisible() == false &&
            Layer_sis_09.getVisible() == false &&
            Layer_sis_10.getVisible() == false &&
            Layer_sis_11.getVisible() == false &&
            Layer_sis_12.getVisible() == false) {
            $('#j1_134_anchor').css('font-weight', '');
        }
        else {
            $('#j1_134_anchor').css('font-weight', 'bold');
            saraeh4 = true;
        }
        if (saraeh1 == false &&
            saraeh2 == false &&
            saraeh3 == false &&
            saraeh4 == false) {
            $('#j1_91_anchor').css('font-weight', '');
        }
        else {
            $('#j1_91_anchor').css('font-weight', 'bold');
            saraeh = true;
        }
        var clara = false;
        if (Layer_sis_klr_year.getVisible() == false &&
            Layer_sis_klr_01.getVisible() == false &&
            Layer_sis_klr_02.getVisible() == false &&
            Layer_sis_klr_03.getVisible() == false &&
            Layer_sis_klr_04.getVisible() == false &&
            Layer_sis_klr_05.getVisible() == false &&
            Layer_sis_klr_06.getVisible() == false &&
            Layer_sis_klr_07.getVisible() == false &&
            Layer_sis_klr_08.getVisible() == false &&
            Layer_sis_klr_09.getVisible() == false &&
            Layer_sis_klr_10.getVisible() == false &&
            Layer_sis_klr_11.getVisible() == false &&
            Layer_sis_klr_12.getVisible() == false) {
            $('#j1_149_anchor').css('font-weight', '');
        }
        else {
            $('#j1_149_anchor').css('font-weight', 'bold');
            clara = true;
        }
        if (nasasse == false &&
            saraeh == false &&
            clara == false) {
            $('#j1_5_anchor').css('font-weight', '');
        }
        else {
            $('#j1_5_anchor').css('font-weight', 'bold');
            saraeh = true;
        }

        var climat1 = false;
        var climat2 = false;
        var climat3 = false;
        var climat4 = false;
        var climat5 = false;
        var climat6 = false;
        var climat7 = false;
        var climat8 = false;
        var climat9 = false;
        var climat10 = false;
        if (Layer_rainavgesm_year.getVisible() == false &&
            Layer_rainavgesm_01.getVisible() == false &&
            Layer_rainavgesm_02.getVisible() == false &&
            Layer_rainavgesm_03.getVisible() == false &&
            Layer_rainavgesm_04.getVisible() == false &&
            Layer_rainavgesm_05.getVisible() == false &&
            Layer_rainavgesm_06.getVisible() == false &&
            Layer_rainavgesm_07.getVisible() == false &&
            Layer_rainavgesm_08.getVisible() == false &&
            Layer_rainavgesm_09.getVisible() == false &&
            Layer_rainavgesm_10.getVisible() == false &&
            Layer_rainavgesm_11.getVisible() == false &&
            Layer_rainavgesm_12.getVisible() == false) {
            $('#j1_164_anchor').css('font-weight', '');
        }
        else {
            $('#j1_164_anchor').css('font-weight', 'bold');
            climat1 = true;
        }
        if (Layer_t10m_01.getVisible() == false &&
            Layer_t10m_02.getVisible() == false &&
            Layer_t10m_03.getVisible() == false &&
            Layer_t10m_04.getVisible() == false &&
            Layer_t10m_05.getVisible() == false &&
            Layer_t10m_06.getVisible() == false &&
            Layer_t10m_07.getVisible() == false &&
            Layer_t10m_08.getVisible() == false &&
            Layer_t10m_09.getVisible() == false &&
            Layer_t10m_10.getVisible() == false &&
            Layer_t10m_11.getVisible() == false &&
            Layer_t10m_12.getVisible() == false) {
            $('#j1_178_anchor').css('font-weight', '');
        }
        else {
            $('#j1_178_anchor').css('font-weight', 'bold');
            climat2 = true;
        }
        if (Layer_t10mmax_01.getVisible() == false &&
            Layer_t10mmax_02.getVisible() == false &&
            Layer_t10mmax_03.getVisible() == false &&
            Layer_t10mmax_04.getVisible() == false &&
            Layer_t10mmax_05.getVisible() == false &&
            Layer_t10mmax_06.getVisible() == false &&
            Layer_t10mmax_07.getVisible() == false &&
            Layer_t10mmax_08.getVisible() == false &&
            Layer_t10mmax_09.getVisible() == false &&
            Layer_t10mmax_10.getVisible() == false &&
            Layer_t10mmax_11.getVisible() == false &&
            Layer_t10mmax_12.getVisible() == false) {
            $('#j1_191_anchor').css('font-weight', '');
        }
        else {
            $('#j1_191_anchor').css('font-weight', 'bold');
            climat3 = true;
        }
        if (Layer_t10m_min_01.getVisible() == false &&
            Layer_t10m_min_02.getVisible() == false &&
            Layer_t10m_min_03.getVisible() == false &&
            Layer_t10m_min_04.getVisible() == false &&
            Layer_t10m_min_05.getVisible() == false &&
            Layer_t10m_min_06.getVisible() == false &&
            Layer_t10m_min_07.getVisible() == false &&
            Layer_t10m_min_08.getVisible() == false &&
            Layer_t10m_min_09.getVisible() == false &&
            Layer_t10m_min_10.getVisible() == false &&
            Layer_t10m_min_11.getVisible() == false &&
            Layer_t10m_min_12.getVisible() == false) {
            $('#j1_204_anchor').css('font-weight', '');
        }
        else {
            $('#j1_204_anchor').css('font-weight', 'bold');
            climat4 = true;
        }
        if (Layer_tskinavg_01.getVisible() == false &&
            Layer_tskinavg_02.getVisible() == false &&
            Layer_tskinavg_03.getVisible() == false &&
            Layer_tskinavg_04.getVisible() == false &&
            Layer_tskinavg_05.getVisible() == false &&
            Layer_tskinavg_06.getVisible() == false &&
            Layer_tskinavg_07.getVisible() == false &&
            Layer_tskinavg_08.getVisible() == false &&
            Layer_tskinavg_09.getVisible() == false &&
            Layer_tskinavg_10.getVisible() == false &&
            Layer_tskinavg_11.getVisible() == false &&
            Layer_tskinavg_12.getVisible() == false) {
            $('#j1_217_anchor').css('font-weight', '');
        }
        else {
            $('#j1_217_anchor').css('font-weight', 'bold');
            climat5 = true;
        }
        if (Layer_srfalbavg_01.getVisible() == false &&
            Layer_srfalbavg_02.getVisible() == false &&
            Layer_srfalbavg_03.getVisible() == false &&
            Layer_srfalbavg_04.getVisible() == false &&
            Layer_srfalbavg_05.getVisible() == false &&
            Layer_srfalbavg_06.getVisible() == false &&
            Layer_srfalbavg_07.getVisible() == false &&
            Layer_srfalbavg_08.getVisible() == false &&
            Layer_srfalbavg_09.getVisible() == false &&
            Layer_srfalbavg_10.getVisible() == false &&
            Layer_srfalbavg_11.getVisible() == false &&
            Layer_srfalbavg_12.getVisible() == false) {
            $('#j1_230_anchor').css('font-weight', '');
        }
        else {
            $('#j1_230_anchor').css('font-weight', 'bold');
            climat6 = true;
        }
        if (Layer_avg_kt_22_01.getVisible() == false &&
            Layer_avg_kt_22_02.getVisible() == false &&
            Layer_avg_kt_22_03.getVisible() == false &&
            Layer_avg_kt_22_04.getVisible() == false &&
            Layer_avg_kt_22_05.getVisible() == false &&
            Layer_avg_kt_22_06.getVisible() == false &&
            Layer_avg_kt_22_07.getVisible() == false &&
            Layer_avg_kt_22_08.getVisible() == false &&
            Layer_avg_kt_22_09.getVisible() == false &&
            Layer_avg_kt_22_10.getVisible() == false &&
            Layer_avg_kt_22_11.getVisible() == false &&
            Layer_avg_kt_22_12.getVisible() == false) {
            $('#j1_243_anchor').css('font-weight', '');
        }
        else {
            $('#j1_243_anchor').css('font-weight', 'bold');
            climat7 = true;
        }
        if (Layer_avg_nkt_22_01.getVisible() == false &&
            Layer_avg_nkt_22_02.getVisible() == false &&
            Layer_avg_nkt_22_03.getVisible() == false &&
            Layer_avg_nkt_22_04.getVisible() == false &&
            Layer_avg_nkt_22_05.getVisible() == false &&
            Layer_avg_nkt_22_06.getVisible() == false &&
            Layer_avg_nkt_22_07.getVisible() == false &&
            Layer_avg_nkt_22_08.getVisible() == false &&
            Layer_avg_nkt_22_09.getVisible() == false &&
            Layer_avg_nkt_22_10.getVisible() == false &&
            Layer_avg_nkt_22_11.getVisible() == false &&
            Layer_avg_nkt_22_12.getVisible() == false) {
            $('#j1_256_anchor').css('font-weight', '');
        }
        else {
            $('#j1_256_anchor').css('font-weight', 'bold');
            climat8 = true;
        }
        if (Layer_day_cld_22_01.getVisible() == false &&
            Layer_day_cld_22_02.getVisible() == false &&
            Layer_day_cld_22_03.getVisible() == false &&
            Layer_day_cld_22_04.getVisible() == false &&
            Layer_day_cld_22_05.getVisible() == false &&
            Layer_day_cld_22_06.getVisible() == false &&
            Layer_day_cld_22_07.getVisible() == false &&
            Layer_day_cld_22_08.getVisible() == false &&
            Layer_day_cld_22_09.getVisible() == false &&
            Layer_day_cld_22_10.getVisible() == false &&
            Layer_day_cld_22_11.getVisible() == false &&
            Layer_day_cld_22_12.getVisible() == false) {
            $('#j1_269_anchor').css('font-weight', '');
        }
        else {
            $('#j1_269_anchor').css('font-weight', 'bold');
            climat9 = true;
        }
        if (Layer_daylghtav_01.getVisible() == false &&
            Layer_daylghtav_02.getVisible() == false &&
            Layer_daylghtav_03.getVisible() == false &&
            Layer_daylghtav_04.getVisible() == false &&
            Layer_daylghtav_05.getVisible() == false &&
            Layer_daylghtav_06.getVisible() == false &&
            Layer_daylghtav_07.getVisible() == false &&
            Layer_daylghtav_08.getVisible() == false &&
            Layer_daylghtav_09.getVisible() == false &&
            Layer_daylghtav_10.getVisible() == false &&
            Layer_daylghtav_11.getVisible() == false &&
            Layer_daylghtav_12.getVisible() == false) {
            $('#j1_282_anchor').css('font-weight', '');
        }
        else {
            $('#j1_282_anchor').css('font-weight', 'bold');
            climat10 = true;
        }
        if (climat1 == false &&
            climat2 == false &&
            climat3 == false &&
            climat4 == false &&
            climat5 == false &&
            climat6 == false &&
            climat7 == false &&
            climat8 == false &&
            climat9 == false &&
            climat10 == false) {
            $('#j1_163_anchor').css('font-weight', '');
        }
        else {
            $('#j1_163_anchor').css('font-weight', 'bold');
        }
        var factor1 = false;
        var factor2 = false;
        if (Layer_srtm.getVisible() == false &&
            Layer_aspect_srtm.getVisible() == false &&
            Layer_slope_srtm.getVisible() == false) {
            $('#j1_296_anchor').css('font-weight', '');
        }
        else {
            $('#j1_296_anchor').css('font-weight', 'bold');
            factor1 = true;
        }
        if (Layer_pamyatnikprirodypol.getVisible() == false &&
            Layer_prirparki.getVisible() == false &&
            Layer_rezervaty.getVisible() == false &&
            Layer_zakazniky.getVisible() == false &&
            Layer_zapovedniki.getVisible() == false &&
            Layer_zapovedzony.getVisible() == false) {
            $('#j1_300_anchor').css('font-weight', '');
        }
        else {
            $('#j1_300_anchor').css('font-weight', 'bold');
            factor2 = true;
        }
        if (factor1 == false &&
            factor2 == false &&
            Layer_hidroohrzony.getVisible() == false &&
            Layer_kzcover.getVisible() == false &&
            Layer_meteo_st.getVisible() == false) {
            $('#j1_295_anchor').css('font-weight', '');
        }
        else {
            $('#j1_295_anchor').css('font-weight', 'bold');
        }
        // legends
        if (Layer_spp.getVisible() == false) {
            $('#legend_spp').hide();
        }
        else {
            $('#legend_spp').show();
        }
        if (Layer_lep.getVisible() == false) {
            $('#legend_lep').hide();
        }
        else {
            $('#legend_lep').show();
        }
        if (Layer_lep2.getVisible() == false) {
            $('#legend_lep2').hide();
        }
        else {
            $('#legend_lep2').show();
        }
        if (Layer_srtm.getVisible() == false) {
            $('#legend_srtm').hide();
        }
        else {
            $('#legend_srtm').show();
        }
        if (Layer_aspect_srtm.getVisible() == false) {
            $('#legend_aspect_srtm').hide();
        }
        else {
            $('#legend_aspect_srtm').show();
        }
        if (Layer_slope_srtm.getVisible() == false) {
            $('#legend_slope_srtm').hide();
        }
        else {
            $('#legend_slope_srtm').show();
        }
        if (Layer_avg_dnr_year.getVisible() == false) {
            $('#legend_avg_dnr_year').hide();
        }
        else {
            $('#legend_avg_dnr_year').show();
        }
        if (Layer_avg_dnr_01.getVisible() == false &&
            Layer_avg_dnr_02.getVisible() == false &&
            Layer_avg_dnr_03.getVisible() == false &&
            Layer_avg_dnr_04.getVisible() == false &&
            Layer_avg_dnr_05.getVisible() == false &&
            Layer_avg_dnr_06.getVisible() == false &&
            Layer_avg_dnr_07.getVisible() == false &&
            Layer_avg_dnr_08.getVisible() == false &&
            Layer_avg_dnr_09.getVisible() == false &&
            Layer_avg_dnr_10.getVisible() == false &&
            Layer_avg_dnr_11.getVisible() == false &&
            Layer_avg_dnr_12.getVisible() == false) {
            $('#legend_avg_dnr_xx').hide();
        }
        else {
            $('#legend_avg_dnr_xx').show();
        }

        if (Layer_swv_dwn_year.getVisible() == false) {
            $('#legend_swv_dwn_year').hide();
        }
        else {
            $('#legend_swv_dwn_year').show();
        }
        if (Layer_swv_dwn_01.getVisible() == false &&
            Layer_swv_dwn_02.getVisible() == false &&
            Layer_swv_dwn_03.getVisible() == false &&
            Layer_swv_dwn_04.getVisible() == false &&
            Layer_swv_dwn_05.getVisible() == false &&
            Layer_swv_dwn_06.getVisible() == false &&
            Layer_swv_dwn_07.getVisible() == false &&
            Layer_swv_dwn_08.getVisible() == false &&
            Layer_swv_dwn_09.getVisible() == false &&
            Layer_swv_dwn_10.getVisible() == false &&
            Layer_swv_dwn_11.getVisible() == false &&
            Layer_swv_dwn_12.getVisible() == false) {
            $('#legend_swv_dwn_xx').hide();
        }
        else {
            $('#legend_swv_dwn_xx').show();
        }

        if (Layer_exp_dif_year.getVisible() == false) {
            $('#legend_exp_dif_year').hide();
        }
        else {
            $('#legend_exp_dif_year').show();
        }
        if (Layer_exp_dif_01.getVisible() == false &&
            Layer_exp_dif_02.getVisible() == false &&
            Layer_exp_dif_03.getVisible() == false &&
            Layer_exp_dif_04.getVisible() == false &&
            Layer_exp_dif_05.getVisible() == false &&
            Layer_exp_dif_06.getVisible() == false &&
            Layer_exp_dif_07.getVisible() == false &&
            Layer_exp_dif_08.getVisible() == false &&
            Layer_exp_dif_09.getVisible() == false &&
            Layer_exp_dif_10.getVisible() == false &&
            Layer_exp_dif_11.getVisible() == false &&
            Layer_exp_dif_12.getVisible() == false) {
            $('#legend_exp_dif_xx').hide();
        }
        else {
            $('#legend_exp_dif_xx').show();
        }

        if (Layer_rettlt0opt_year.getVisible() == false) {
            $('#legend_rettlt0opt_year').hide();
        }
        else {
            $('#legend_rettlt0opt_year').show();
        }
        if (Layer_rettlt0opt_01.getVisible() == false &&
            Layer_rettlt0opt_02.getVisible() == false &&
            Layer_rettlt0opt_03.getVisible() == false &&
            Layer_rettlt0opt_04.getVisible() == false &&
            Layer_rettlt0opt_05.getVisible() == false &&
            Layer_rettlt0opt_06.getVisible() == false &&
            Layer_rettlt0opt_07.getVisible() == false &&
            Layer_rettlt0opt_08.getVisible() == false &&
            Layer_rettlt0opt_09.getVisible() == false &&
            Layer_rettlt0opt_10.getVisible() == false &&
            Layer_rettlt0opt_11.getVisible() == false &&
            Layer_rettlt0opt_12.getVisible() == false) {
            $('#legend_rettlt0opt_xx').hide();
        }
        else {
            $('#legend_rettlt0opt_xx').show();
        }

        if (Layer_clrskyavrg_year.getVisible() == false) {
            $('#legend_clrskyavrg_year').hide();
        }
        else {
            $('#legend_clrskyavrg_year').show();
        }
        if (Layer_clrskyavrg_01.getVisible() == false &&
            Layer_clrskyavrg_02.getVisible() == false &&
            Layer_clrskyavrg_03.getVisible() == false &&
            Layer_clrskyavrg_04.getVisible() == false &&
            Layer_clrskyavrg_05.getVisible() == false &&
            Layer_clrskyavrg_06.getVisible() == false &&
            Layer_clrskyavrg_07.getVisible() == false &&
            Layer_clrskyavrg_08.getVisible() == false &&
            Layer_clrskyavrg_09.getVisible() == false &&
            Layer_clrskyavrg_10.getVisible() == false &&
            Layer_clrskyavrg_11.getVisible() == false &&
            Layer_clrskyavrg_12.getVisible() == false) {
            $('#legend_clrskyavrg_xx').hide();
        }
        else {
            $('#legend_clrskyavrg_xx').show();
        }

        if (Layer_retesh0mim_year.getVisible() == false) {
            $('#legend_retesh0mim_year').hide();
        }
        else {
            $('#legend_retesh0mim_year').show();
        }
        if (Layer_retesh0mim_01.getVisible() == false &&
            Layer_retesh0mim_02.getVisible() == false &&
            Layer_retesh0mim_03.getVisible() == false &&
            Layer_retesh0mim_04.getVisible() == false &&
            Layer_retesh0mim_05.getVisible() == false &&
            Layer_retesh0mim_06.getVisible() == false &&
            Layer_retesh0mim_07.getVisible() == false &&
            Layer_retesh0mim_08.getVisible() == false &&
            Layer_retesh0mim_09.getVisible() == false &&
            Layer_retesh0mim_10.getVisible() == false &&
            Layer_retesh0mim_11.getVisible() == false &&
            Layer_retesh0mim_12.getVisible() == false) {
            $('#legend_retesh0mim_xx').hide();
        }
        else {
            $('#legend_retesh0mim_xx').show();
        }

        if (Layer_t10m_01.getVisible() == false &&
            Layer_t10m_02.getVisible() == false &&
            Layer_t10m_03.getVisible() == false &&
            Layer_t10m_04.getVisible() == false &&
            Layer_t10m_05.getVisible() == false &&
            Layer_t10m_06.getVisible() == false &&
            Layer_t10m_07.getVisible() == false &&
            Layer_t10m_08.getVisible() == false &&
            Layer_t10m_09.getVisible() == false &&
            Layer_t10m_10.getVisible() == false &&
            Layer_t10m_11.getVisible() == false &&
            Layer_t10m_12.getVisible() == false) {
            $('#legend_t10m_xx').hide();
        }
        else {
            $('#legend_t10m_xx').show();
        }
        if (Layer_rainavgesm_year.getVisible() == false) {
            $('#legend_rainavgesm_year').hide();
        }
        else {
            $('#legend_rainavgesm_year').show();
        }
        if (Layer_rainavgesm_01.getVisible() == false &&
            Layer_rainavgesm_02.getVisible() == false &&
            Layer_rainavgesm_03.getVisible() == false &&
            Layer_rainavgesm_04.getVisible() == false &&
            Layer_rainavgesm_05.getVisible() == false &&
            Layer_rainavgesm_06.getVisible() == false &&
            Layer_rainavgesm_07.getVisible() == false &&
            Layer_rainavgesm_08.getVisible() == false &&
            Layer_rainavgesm_09.getVisible() == false &&
            Layer_rainavgesm_10.getVisible() == false &&
            Layer_rainavgesm_11.getVisible() == false &&
            Layer_rainavgesm_12.getVisible() == false) {
            $('#legend_rainavgesm_xx').hide();
        }
        else {
            $('#legend_rainavgesm_xx').show();
        }
        if (Layer_t10mmax_01.getVisible() == false &&
            Layer_t10mmax_02.getVisible() == false &&
            Layer_t10mmax_03.getVisible() == false &&
            Layer_t10mmax_04.getVisible() == false &&
            Layer_t10mmax_05.getVisible() == false &&
            Layer_t10mmax_06.getVisible() == false &&
            Layer_t10mmax_07.getVisible() == false &&
            Layer_t10mmax_08.getVisible() == false &&
            Layer_t10mmax_09.getVisible() == false &&
            Layer_t10mmax_10.getVisible() == false &&
            Layer_t10mmax_11.getVisible() == false &&
            Layer_t10mmax_12.getVisible() == false) {
            $('#legend_t10mmax_xx').hide();
        }
        else {
            $('#legend_t10mmax_xx').show();
        }
        if (Layer_t10m_min_01.getVisible() == false &&
            Layer_t10m_min_02.getVisible() == false &&
            Layer_t10m_min_03.getVisible() == false &&
            Layer_t10m_min_04.getVisible() == false &&
            Layer_t10m_min_05.getVisible() == false &&
            Layer_t10m_min_06.getVisible() == false &&
            Layer_t10m_min_07.getVisible() == false &&
            Layer_t10m_min_08.getVisible() == false &&
            Layer_t10m_min_09.getVisible() == false &&
            Layer_t10m_min_10.getVisible() == false &&
            Layer_t10m_min_11.getVisible() == false &&
            Layer_t10m_min_12.getVisible() == false) {
            $('#legend_t10m_min_xx').hide();
        }
        else {
            $('#legend_t10m_min_xx').show();
        }
        if (Layer_tskinavg_01.getVisible() == false &&
            Layer_tskinavg_02.getVisible() == false &&
            Layer_tskinavg_03.getVisible() == false &&
            Layer_tskinavg_04.getVisible() == false &&
            Layer_tskinavg_05.getVisible() == false &&
            Layer_tskinavg_06.getVisible() == false &&
            Layer_tskinavg_07.getVisible() == false &&
            Layer_tskinavg_08.getVisible() == false &&
            Layer_tskinavg_09.getVisible() == false &&
            Layer_tskinavg_10.getVisible() == false &&
            Layer_tskinavg_11.getVisible() == false &&
            Layer_tskinavg_12.getVisible() == false) {
            $('#legend_tskinavg_xx').hide();
        }
        else {
            $('#legend_tskinavg_xx').show();
        }
        if (Layer_srfalbavg_01.getVisible() == false &&
            Layer_srfalbavg_02.getVisible() == false &&
            Layer_srfalbavg_03.getVisible() == false &&
            Layer_srfalbavg_04.getVisible() == false &&
            Layer_srfalbavg_05.getVisible() == false &&
            Layer_srfalbavg_06.getVisible() == false &&
            Layer_srfalbavg_07.getVisible() == false &&
            Layer_srfalbavg_08.getVisible() == false &&
            Layer_srfalbavg_09.getVisible() == false &&
            Layer_srfalbavg_10.getVisible() == false &&
            Layer_srfalbavg_11.getVisible() == false &&
            Layer_srfalbavg_12.getVisible() == false) {
            $('#legend_srfalbavg_xx').hide();
        }
        else {
            $('#legend_srfalbavg_xx').show();
        }

        if (Layer_avg_kt_22_01.getVisible() == false &&
            Layer_avg_kt_22_02.getVisible() == false &&
            Layer_avg_kt_22_03.getVisible() == false &&
            Layer_avg_kt_22_04.getVisible() == false &&
            Layer_avg_kt_22_05.getVisible() == false &&
            Layer_avg_kt_22_06.getVisible() == false &&
            Layer_avg_kt_22_07.getVisible() == false &&
            Layer_avg_kt_22_08.getVisible() == false &&
            Layer_avg_kt_22_09.getVisible() == false &&
            Layer_avg_kt_22_10.getVisible() == false &&
            Layer_avg_kt_22_11.getVisible() == false &&
            Layer_avg_kt_22_12.getVisible() == false) {
            $('#legend_avg_kt_22_xx').hide();
        }
        else {
            $('#legend_avg_kt_22_xx').show();
        }
        if (Layer_avg_nkt_22_01.getVisible() == false &&
            Layer_avg_nkt_22_02.getVisible() == false &&
            Layer_avg_nkt_22_03.getVisible() == false &&
            Layer_avg_nkt_22_04.getVisible() == false &&
            Layer_avg_nkt_22_05.getVisible() == false &&
            Layer_avg_nkt_22_06.getVisible() == false &&
            Layer_avg_nkt_22_07.getVisible() == false &&
            Layer_avg_nkt_22_08.getVisible() == false &&
            Layer_avg_nkt_22_09.getVisible() == false &&
            Layer_avg_nkt_22_10.getVisible() == false &&
            Layer_avg_nkt_22_11.getVisible() == false &&
            Layer_avg_nkt_22_12.getVisible() == false) {
            $('#legend_avg_nkt_22_xx').hide();
        }
        else {
            $('#legend_avg_nkt_22_xx').show();
        }
        if (Layer_day_cld_22_01.getVisible() == false &&
            Layer_day_cld_22_02.getVisible() == false &&
            Layer_day_cld_22_03.getVisible() == false &&
            Layer_day_cld_22_04.getVisible() == false &&
            Layer_day_cld_22_05.getVisible() == false &&
            Layer_day_cld_22_06.getVisible() == false &&
            Layer_day_cld_22_07.getVisible() == false &&
            Layer_day_cld_22_08.getVisible() == false &&
            Layer_day_cld_22_09.getVisible() == false &&
            Layer_day_cld_22_10.getVisible() == false &&
            Layer_day_cld_22_11.getVisible() == false &&
            Layer_day_cld_22_12.getVisible() == false) {
            $('#legend_day_cld_22_xx').hide();
        }
        else {
            $('#legend_day_cld_22_xx').show();
        }
        if (Layer_daylghtav_01.getVisible() == false &&
            Layer_daylghtav_02.getVisible() == false &&
            Layer_daylghtav_03.getVisible() == false &&
            Layer_daylghtav_04.getVisible() == false &&
            Layer_daylghtav_05.getVisible() == false &&
            Layer_daylghtav_06.getVisible() == false &&
            Layer_daylghtav_07.getVisible() == false &&
            Layer_daylghtav_08.getVisible() == false &&
            Layer_daylghtav_09.getVisible() == false &&
            Layer_daylghtav_10.getVisible() == false &&
            Layer_daylghtav_11.getVisible() == false &&
            Layer_daylghtav_12.getVisible() == false) {
            $('#legend_daylghtav_xx').hide();
        }
        else {
            $('#legend_daylghtav_xx').show();
        }

        if (Layer_p_clr_cky_year.getVisible() == false) {
            $('#legend_p_clr_cky_year').hide();
        }
        else {
            $('#legend_p_clr_cky_year').show();
        }
        if (Layer_p_clr_cky_01.getVisible() == false &&
            Layer_p_clr_cky_02.getVisible() == false &&
            Layer_p_clr_cky_03.getVisible() == false &&
            Layer_p_clr_cky_04.getVisible() == false &&
            Layer_p_clr_cky_05.getVisible() == false &&
            Layer_p_clr_cky_06.getVisible() == false &&
            Layer_p_clr_cky_07.getVisible() == false &&
            Layer_p_clr_cky_08.getVisible() == false &&
            Layer_p_clr_cky_09.getVisible() == false &&
            Layer_p_clr_cky_10.getVisible() == false &&
            Layer_p_clr_cky_11.getVisible() == false &&
            Layer_p_clr_cky_12.getVisible() == false) {
            $('#legend_p_clr_cky_xx').hide();
        }
        else {
            $('#legend_p_clr_cky_xx').show();
        }
        if (Layer_p_swv_dwn_year.getVisible() == false) {
            $('#legend_p_swv_dwn_year').hide();
        }
        else {
            $('#legend_p_swv_dwn_year').show();
        }
        if (Layer_p_swv_dwn_01.getVisible() == false &&
            Layer_p_swv_dwn_02.getVisible() == false &&
            Layer_p_swv_dwn_03.getVisible() == false &&
            Layer_p_swv_dwn_04.getVisible() == false &&
            Layer_p_swv_dwn_05.getVisible() == false &&
            Layer_p_swv_dwn_06.getVisible() == false &&
            Layer_p_swv_dwn_07.getVisible() == false &&
            Layer_p_swv_dwn_08.getVisible() == false &&
            Layer_p_swv_dwn_09.getVisible() == false &&
            Layer_p_swv_dwn_10.getVisible() == false &&
            Layer_p_swv_dwn_11.getVisible() == false &&
            Layer_p_swv_dwn_12.getVisible() == false) {
            $('#legend_p_swv_dwn_xx').hide();
        }
        else {
            $('#legend_p_swv_dwn_xx').show();
        }
        if (Layer_p_toa_dwn_year.getVisible() == false) {
            $('#legend_p_toa_dwn_year').hide();
        }
        else {
            $('#legend_p_toa_dwn_year').show();
        }
        if (Layer_p_toa_dwn_01.getVisible() == false &&
            Layer_p_toa_dwn_02.getVisible() == false &&
            Layer_p_toa_dwn_03.getVisible() == false &&
            Layer_p_toa_dwn_04.getVisible() == false &&
            Layer_p_toa_dwn_05.getVisible() == false &&
            Layer_p_toa_dwn_06.getVisible() == false &&
            Layer_p_toa_dwn_07.getVisible() == false &&
            Layer_p_toa_dwn_08.getVisible() == false &&
            Layer_p_toa_dwn_09.getVisible() == false &&
            Layer_p_toa_dwn_10.getVisible() == false &&
            Layer_p_toa_dwn_11.getVisible() == false &&
            Layer_p_toa_dwn_12.getVisible() == false) {
            $('#legend_p_toa_dwn_xx').hide();
        }
        else {
            $('#legend_p_toa_dwn_xx').show();
        }

        if (Layer_dni_year.getVisible() == false) {
            $('#legend_dni_year').hide();
        }
        else {
            $('#legend_dni_year').show();
        }
        if (Layer_dni_01.getVisible() == false &&
            Layer_dni_02.getVisible() == false &&
            Layer_dni_03.getVisible() == false &&
            Layer_dni_04.getVisible() == false &&
            Layer_dni_05.getVisible() == false &&
            Layer_dni_06.getVisible() == false &&
            Layer_dni_07.getVisible() == false &&
            Layer_dni_08.getVisible() == false &&
            Layer_dni_09.getVisible() == false &&
            Layer_dni_10.getVisible() == false &&
            Layer_dni_11.getVisible() == false &&
            Layer_dni_12.getVisible() == false) {
            $('#legend_dni_xx').hide();
        }
        else {
            $('#legend_dni_xx').show();
        }

        if (Layer_sic_year.getVisible() == false) {
            $('#legend_sic_year').hide();
        }
        else {
            $('#legend_sic_year').show();
        }
        if (Layer_sic_01.getVisible() == false &&
            Layer_sic_02.getVisible() == false &&
            Layer_sic_03.getVisible() == false &&
            Layer_sic_04.getVisible() == false &&
            Layer_sic_05.getVisible() == false &&
            Layer_sic_06.getVisible() == false &&
            Layer_sic_07.getVisible() == false &&
            Layer_sic_08.getVisible() == false &&
            Layer_sic_09.getVisible() == false &&
            Layer_sic_10.getVisible() == false &&
            Layer_sic_11.getVisible() == false &&
            Layer_sic_12.getVisible() == false) {
            $('#legend_sic_xx').hide();
        }
        else {
            $('#legend_sic_xx').show();
        }

        if (Layer_sid_year.getVisible() == false) {
            $('#legend_sid_year').hide();
        }
        else {
            $('#legend_sid_year').show();
        }
        if (Layer_sid_01.getVisible() == false &&
            Layer_sid_02.getVisible() == false &&
            Layer_sid_03.getVisible() == false &&
            Layer_sid_04.getVisible() == false &&
            Layer_sid_05.getVisible() == false &&
            Layer_sid_06.getVisible() == false &&
            Layer_sid_07.getVisible() == false &&
            Layer_sid_08.getVisible() == false &&
            Layer_sid_09.getVisible() == false &&
            Layer_sid_10.getVisible() == false &&
            Layer_sid_11.getVisible() == false &&
            Layer_sid_12.getVisible() == false) {
            $('#legend_sid_xx').hide();
        }
        else {
            $('#legend_sid_xx').show();
        }

        if (Layer_sis_year.getVisible() == false) {
            $('#legend_sis_year').hide();
        }
        else {
            $('#legend_sis_year').show();
        }
        if (Layer_sis_01.getVisible() == false &&
            Layer_sis_02.getVisible() == false &&
            Layer_sis_03.getVisible() == false &&
            Layer_sis_04.getVisible() == false &&
            Layer_sis_05.getVisible() == false &&
            Layer_sis_06.getVisible() == false &&
            Layer_sis_07.getVisible() == false &&
            Layer_sis_08.getVisible() == false &&
            Layer_sis_09.getVisible() == false &&
            Layer_sis_10.getVisible() == false &&
            Layer_sis_11.getVisible() == false &&
            Layer_sis_12.getVisible() == false) {
            $('#legend_sis_xx').hide();
        }
        else {
            $('#legend_sis_xx').show();
        }

        if (Layer_sis_klr_year.getVisible() == false) {
            $('#legend_sis_klr_year').hide();
        }
        else {
            $('#legend_sis_klr_year').show();
        }
        if (Layer_sis_klr_01.getVisible() == false &&
            Layer_sis_klr_02.getVisible() == false &&
            Layer_sis_klr_03.getVisible() == false &&
            Layer_sis_klr_04.getVisible() == false &&
            Layer_sis_klr_05.getVisible() == false &&
            Layer_sis_klr_06.getVisible() == false &&
            Layer_sis_klr_07.getVisible() == false &&
            Layer_sis_klr_08.getVisible() == false &&
            Layer_sis_klr_09.getVisible() == false &&
            Layer_sis_klr_10.getVisible() == false &&
            Layer_sis_klr_11.getVisible() == false &&
            Layer_sis_klr_12.getVisible() == false) {
            $('#legend_sis_klr_xx').hide();
        }
        else {
            $('#legend_sis_klr_xx').show();
        }

        if (Layer_pamyatnikprirodypol.getVisible() == false &&
            Layer_prirparki.getVisible() == false &&
            Layer_rezervaty.getVisible() == false &&
            Layer_zakazniky.getVisible() == false &&
            Layer_zapovedniki.getVisible() == false &&
            Layer_zapovedzony.getVisible() == false) {
            $('#legend_oopt').hide();
        }
        else {
            $('#legend_oopt').show();
        }
        if (Layer_hidroohrzony.getVisible() == false) {
            $('#legend_hidroohrzony').hide();
        }
        else {
            $('#legend_hidroohrzony').show();
        }
        if (Layer_arheopamyat.getVisible() == false) {
            $('#legend_arheopamyat').hide();
        }
        else {
            $('#legend_arheopamyat').show();
        }
        if (Layer_kzcover.getVisible() == false) {
            $('#legend_kzcover').hide();
        }
        else {
            $('#legend_kzcover').show();
        }
        });
    if ($('#mapid').html() == "avg_dnr") {
        $.jstree.reference('#jstree').check_node('swv_dwn_year');
    }
    if ($('#mapid').html() == "factor") {
        $.jstree.reference('#jstree').check_node('pamyatnikprirodypol');
        $.jstree.reference('#jstree').check_node('prirparki');
        $.jstree.reference('#jstree').check_node('rezervaty');
        $.jstree.reference('#jstree').check_node('zakazniky');
        $.jstree.reference('#jstree').check_node('zapovedniki');
        $.jstree.reference('#jstree').check_node('zapovedzony');
    }
    if ($('#mapid').html() == "t10m") {
        $.jstree.reference('#jstree').check_node('t10m_01');
    }
    if ($('#mapid').html() == "solar") {
        $.jstree.reference('#jstree').check_node('lep');
    }

    $.jstree.reference('#jstree').check_node('oblasti');
    $.jstree.reference('#jstree').check_node('spp');
});