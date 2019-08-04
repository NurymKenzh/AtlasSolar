// функция нажатия на карту
map.on('singleclick', function (evt) {
    var coordinates = ol.proj.transform(evt.coordinate, 'EPSG:3857', 'EPSG:4326');
    longitude = coordinates[0];
    latitude = coordinates[1];
    var coordinate = evt.coordinate;
    Source_select.clear();
    if (tool == 'info' || tool == 'analizeterrain') {
        var pixel = map.getEventPixel(evt.originalEvent);
        ///////////////////////////////////
        $("#dialog_table").find("tr:gt(0)").remove();
        var viewResolution = (view.getResolution());
        var url_oblasti = Source_oblasti.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'name_rs,name_kz,name_an' });
        var url_rayony = Source_rayony.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'name_rs,name_kz,name_an' });

        var url_analizeterrain = Source_AnalizeTerrain.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_lep = Source_lep.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', {
            'INFO_FORMAT': 'text/javascript',
            'propertyName': 'geom,gid,name_rs,vltg,rng,type,dtntr,elvt,mtrl,insttp,lnght,rnzon',
            'FEATURE_COUNT': '20'
        });

        var url_srtm = Source_srtm.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_aspect_srtm = Source_aspect_srtm.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_slope_srtm = Source_slope_srtm.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_avg_dnr_year = Source_avg_dnr_year.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_dnr_01 = Source_avg_dnr_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_dnr_02 = Source_avg_dnr_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_dnr_03 = Source_avg_dnr_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_dnr_04 = Source_avg_dnr_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_dnr_05 = Source_avg_dnr_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_dnr_06 = Source_avg_dnr_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_dnr_07 = Source_avg_dnr_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_dnr_08 = Source_avg_dnr_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_dnr_09 = Source_avg_dnr_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_dnr_10 = Source_avg_dnr_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_dnr_11 = Source_avg_dnr_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_dnr_12 = Source_avg_dnr_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_swv_dwn_year = Source_swv_dwn_year.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_swv_dwn_01 = Source_swv_dwn_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_swv_dwn_02 = Source_swv_dwn_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_swv_dwn_03 = Source_swv_dwn_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_swv_dwn_04 = Source_swv_dwn_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_swv_dwn_05 = Source_swv_dwn_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_swv_dwn_06 = Source_swv_dwn_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_swv_dwn_07 = Source_swv_dwn_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_swv_dwn_08 = Source_swv_dwn_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_swv_dwn_09 = Source_swv_dwn_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_swv_dwn_10 = Source_swv_dwn_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_swv_dwn_11 = Source_swv_dwn_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_swv_dwn_12 = Source_swv_dwn_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_exp_dif_year = Source_exp_dif_year.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_exp_dif_01 = Source_exp_dif_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_exp_dif_02 = Source_exp_dif_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_exp_dif_03 = Source_exp_dif_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_exp_dif_04 = Source_exp_dif_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_exp_dif_05 = Source_exp_dif_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_exp_dif_06 = Source_exp_dif_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_exp_dif_07 = Source_exp_dif_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_exp_dif_08 = Source_exp_dif_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_exp_dif_09 = Source_exp_dif_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_exp_dif_10 = Source_exp_dif_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_exp_dif_11 = Source_exp_dif_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_exp_dif_12 = Source_exp_dif_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_rettlt0opt_year = Source_rettlt0opt_year.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rettlt0opt_01 = Source_rettlt0opt_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rettlt0opt_02 = Source_rettlt0opt_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rettlt0opt_03 = Source_rettlt0opt_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rettlt0opt_04 = Source_rettlt0opt_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rettlt0opt_05 = Source_rettlt0opt_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rettlt0opt_06 = Source_rettlt0opt_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rettlt0opt_07 = Source_rettlt0opt_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rettlt0opt_08 = Source_rettlt0opt_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rettlt0opt_09 = Source_rettlt0opt_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rettlt0opt_10 = Source_rettlt0opt_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rettlt0opt_11 = Source_rettlt0opt_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rettlt0opt_12 = Source_rettlt0opt_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_clrskyavrg_year = Source_clrskyavrg_year.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_clrskyavrg_01 = Source_clrskyavrg_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_clrskyavrg_02 = Source_clrskyavrg_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_clrskyavrg_03 = Source_clrskyavrg_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_clrskyavrg_04 = Source_clrskyavrg_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_clrskyavrg_05 = Source_clrskyavrg_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_clrskyavrg_06 = Source_clrskyavrg_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_clrskyavrg_07 = Source_clrskyavrg_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_clrskyavrg_08 = Source_clrskyavrg_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_clrskyavrg_09 = Source_clrskyavrg_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_clrskyavrg_10 = Source_clrskyavrg_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_clrskyavrg_11 = Source_clrskyavrg_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_clrskyavrg_12 = Source_clrskyavrg_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_retesh0mim_year = Source_retesh0mim_year.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_retesh0mim_01 = Source_retesh0mim_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_retesh0mim_02 = Source_retesh0mim_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_retesh0mim_03 = Source_retesh0mim_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_retesh0mim_04 = Source_retesh0mim_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_retesh0mim_05 = Source_retesh0mim_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_retesh0mim_06 = Source_retesh0mim_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_retesh0mim_07 = Source_retesh0mim_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_retesh0mim_08 = Source_retesh0mim_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_retesh0mim_09 = Source_retesh0mim_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_retesh0mim_10 = Source_retesh0mim_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_retesh0mim_11 = Source_retesh0mim_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_retesh0mim_12 = Source_retesh0mim_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_t10m_01 = Source_t10m_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_02 = Source_t10m_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_03 = Source_t10m_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_04 = Source_t10m_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_05 = Source_t10m_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_06 = Source_t10m_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_07 = Source_t10m_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_08 = Source_t10m_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_09 = Source_t10m_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_10 = Source_t10m_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_11 = Source_t10m_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_12 = Source_t10m_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_rainavgesm_year = Source_rainavgesm_year.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rainavgesm_01 = Source_rainavgesm_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rainavgesm_02 = Source_rainavgesm_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rainavgesm_03 = Source_rainavgesm_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rainavgesm_04 = Source_rainavgesm_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rainavgesm_05 = Source_rainavgesm_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rainavgesm_06 = Source_rainavgesm_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rainavgesm_07 = Source_rainavgesm_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rainavgesm_08 = Source_rainavgesm_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rainavgesm_09 = Source_rainavgesm_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rainavgesm_10 = Source_rainavgesm_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rainavgesm_11 = Source_rainavgesm_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_rainavgesm_12 = Source_rainavgesm_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_t10mmax_01 = Source_t10mmax_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10mmax_02 = Source_t10mmax_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10mmax_03 = Source_t10mmax_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10mmax_04 = Source_t10mmax_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10mmax_05 = Source_t10mmax_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10mmax_06 = Source_t10mmax_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10mmax_07 = Source_t10mmax_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10mmax_08 = Source_t10mmax_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10mmax_09 = Source_t10mmax_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10mmax_10 = Source_t10mmax_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10mmax_11 = Source_t10mmax_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10mmax_12 = Source_t10mmax_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_t10m_min_01 = Source_t10m_min_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_min_02 = Source_t10m_min_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_min_03 = Source_t10m_min_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_min_04 = Source_t10m_min_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_min_05 = Source_t10m_min_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_min_06 = Source_t10m_min_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_min_07 = Source_t10m_min_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_min_08 = Source_t10m_min_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_min_09 = Source_t10m_min_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_min_10 = Source_t10m_min_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_min_11 = Source_t10m_min_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_t10m_min_12 = Source_t10m_min_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_tskinavg_01 = Source_tskinavg_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_tskinavg_02 = Source_tskinavg_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_tskinavg_03 = Source_tskinavg_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_tskinavg_04 = Source_tskinavg_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_tskinavg_05 = Source_tskinavg_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_tskinavg_06 = Source_tskinavg_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_tskinavg_07 = Source_tskinavg_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_tskinavg_08 = Source_tskinavg_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_tskinavg_09 = Source_tskinavg_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_tskinavg_10 = Source_tskinavg_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_tskinavg_11 = Source_tskinavg_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_tskinavg_12 = Source_tskinavg_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_srfalbavg_01 = Source_srfalbavg_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_srfalbavg_02 = Source_srfalbavg_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_srfalbavg_03 = Source_srfalbavg_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_srfalbavg_04 = Source_srfalbavg_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_srfalbavg_05 = Source_srfalbavg_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_srfalbavg_06 = Source_srfalbavg_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_srfalbavg_07 = Source_srfalbavg_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_srfalbavg_08 = Source_srfalbavg_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_srfalbavg_09 = Source_srfalbavg_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_srfalbavg_10 = Source_srfalbavg_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_srfalbavg_11 = Source_srfalbavg_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_srfalbavg_12 = Source_srfalbavg_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_avg_kt_22_01 = Source_avg_kt_22_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_kt_22_02 = Source_avg_kt_22_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_kt_22_03 = Source_avg_kt_22_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_kt_22_04 = Source_avg_kt_22_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_kt_22_05 = Source_avg_kt_22_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_kt_22_06 = Source_avg_kt_22_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_kt_22_07 = Source_avg_kt_22_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_kt_22_08 = Source_avg_kt_22_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_kt_22_09 = Source_avg_kt_22_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_kt_22_10 = Source_avg_kt_22_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_kt_22_11 = Source_avg_kt_22_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_kt_22_12 = Source_avg_kt_22_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_avg_nkt_22_01 = Source_avg_nkt_22_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_nkt_22_02 = Source_avg_nkt_22_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_nkt_22_03 = Source_avg_nkt_22_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_nkt_22_04 = Source_avg_nkt_22_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_nkt_22_05 = Source_avg_nkt_22_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_nkt_22_06 = Source_avg_nkt_22_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_nkt_22_07 = Source_avg_nkt_22_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_nkt_22_08 = Source_avg_nkt_22_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_nkt_22_09 = Source_avg_nkt_22_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_nkt_22_10 = Source_avg_nkt_22_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_nkt_22_11 = Source_avg_nkt_22_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_avg_nkt_22_12 = Source_avg_nkt_22_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_day_cld_22_01 = Source_day_cld_22_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_day_cld_22_02 = Source_day_cld_22_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_day_cld_22_03 = Source_day_cld_22_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_day_cld_22_04 = Source_day_cld_22_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_day_cld_22_05 = Source_day_cld_22_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_day_cld_22_06 = Source_day_cld_22_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_day_cld_22_07 = Source_day_cld_22_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_day_cld_22_08 = Source_day_cld_22_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_day_cld_22_09 = Source_day_cld_22_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_day_cld_22_10 = Source_day_cld_22_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_day_cld_22_11 = Source_day_cld_22_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_day_cld_22_12 = Source_day_cld_22_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_daylghtav_01 = Source_daylghtav_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_daylghtav_02 = Source_daylghtav_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_daylghtav_03 = Source_daylghtav_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_daylghtav_04 = Source_daylghtav_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_daylghtav_05 = Source_daylghtav_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_daylghtav_06 = Source_daylghtav_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_daylghtav_07 = Source_daylghtav_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_daylghtav_08 = Source_daylghtav_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_daylghtav_09 = Source_daylghtav_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_daylghtav_10 = Source_daylghtav_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_daylghtav_11 = Source_daylghtav_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_daylghtav_12 = Source_daylghtav_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_p_clr_cky_year = Source_p_clr_cky_year.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_clr_cky_01 = Source_p_clr_cky_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_clr_cky_02 = Source_p_clr_cky_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_clr_cky_03 = Source_p_clr_cky_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_clr_cky_04 = Source_p_clr_cky_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_clr_cky_05 = Source_p_clr_cky_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_clr_cky_06 = Source_p_clr_cky_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_clr_cky_07 = Source_p_clr_cky_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_clr_cky_08 = Source_p_clr_cky_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_clr_cky_09 = Source_p_clr_cky_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_clr_cky_10 = Source_p_clr_cky_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_clr_cky_11 = Source_p_clr_cky_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_clr_cky_12 = Source_p_clr_cky_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_p_swv_dwn_year = Source_p_swv_dwn_year.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_swv_dwn_01 = Source_p_swv_dwn_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_swv_dwn_02 = Source_p_swv_dwn_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_swv_dwn_03 = Source_p_swv_dwn_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_swv_dwn_04 = Source_p_swv_dwn_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_swv_dwn_05 = Source_p_swv_dwn_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_swv_dwn_06 = Source_p_swv_dwn_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_swv_dwn_07 = Source_p_swv_dwn_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_swv_dwn_08 = Source_p_swv_dwn_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_swv_dwn_09 = Source_p_swv_dwn_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_swv_dwn_10 = Source_p_swv_dwn_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_swv_dwn_11 = Source_p_swv_dwn_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_swv_dwn_12 = Source_p_swv_dwn_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_p_toa_dwn_year = Source_p_toa_dwn_year.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_toa_dwn_01 = Source_p_toa_dwn_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_toa_dwn_02 = Source_p_toa_dwn_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_toa_dwn_03 = Source_p_toa_dwn_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_toa_dwn_04 = Source_p_toa_dwn_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_toa_dwn_05 = Source_p_toa_dwn_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_toa_dwn_06 = Source_p_toa_dwn_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_toa_dwn_07 = Source_p_toa_dwn_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_toa_dwn_08 = Source_p_toa_dwn_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_toa_dwn_09 = Source_p_toa_dwn_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_toa_dwn_10 = Source_p_toa_dwn_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_toa_dwn_11 = Source_p_toa_dwn_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_p_toa_dwn_12 = Source_p_toa_dwn_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_dni_year = Source_dni_year.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_dni_01 = Source_dni_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_dni_02 = Source_dni_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_dni_03 = Source_dni_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_dni_04 = Source_dni_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_dni_05 = Source_dni_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_dni_06 = Source_dni_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_dni_07 = Source_dni_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_dni_08 = Source_dni_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_dni_09 = Source_dni_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_dni_10 = Source_dni_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_dni_11 = Source_dni_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_dni_12 = Source_dni_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_sic_year = Source_sic_year.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sic_01 = Source_sic_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sic_02 = Source_sic_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sic_03 = Source_sic_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sic_04 = Source_sic_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sic_05 = Source_sic_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sic_06 = Source_sic_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sic_07 = Source_sic_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sic_08 = Source_sic_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sic_09 = Source_sic_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sic_10 = Source_sic_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sic_11 = Source_sic_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sic_12 = Source_sic_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_sid_year = Source_sid_year.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sid_01 = Source_sid_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sid_02 = Source_sid_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sid_03 = Source_sid_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sid_04 = Source_sid_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sid_05 = Source_sid_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sid_06 = Source_sid_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sid_07 = Source_sid_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sid_08 = Source_sid_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sid_09 = Source_sid_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sid_10 = Source_sid_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sid_11 = Source_sid_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sid_12 = Source_sid_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_sis_year = Source_sis_year.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_01 = Source_sis_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_02 = Source_sis_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_03 = Source_sis_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_04 = Source_sis_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_05 = Source_sis_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_06 = Source_sis_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_07 = Source_sis_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_08 = Source_sis_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_09 = Source_sis_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_10 = Source_sis_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_11 = Source_sis_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_12 = Source_sis_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_sis_klr_year = Source_sis_klr_year.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_klr_01 = Source_sis_klr_01.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_klr_02 = Source_sis_klr_02.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_klr_03 = Source_sis_klr_03.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_klr_04 = Source_sis_klr_04.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_klr_05 = Source_sis_klr_05.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_klr_06 = Source_sis_klr_06.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_klr_07 = Source_sis_klr_07.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_klr_08 = Source_sis_klr_08.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_klr_09 = Source_sis_klr_09.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_klr_10 = Source_sis_klr_10.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_klr_11 = Source_sis_klr_11.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });
        var url_sis_klr_12 = Source_sis_klr_12.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_pamyatnikprirodypol = Source_pamyatnikprirodypol.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'name_rs' });
        var url_prirparki = Source_prirparki.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'name_rs' });
        var url_rezervaty = Source_rezervaty.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'name_rs' });
        var url_zakazniky = Source_zakazniky.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'name_rs' });
        var url_zapovedniki = Source_zapovedniki.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'name_rs' });
        var url_zapovedzony = Source_zapovedzony.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'name_rs' });

        var url_hidroohrzony = Source_hidroohrzony.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'name_rs' });
        var url_arheopamyat = Source_arheopamyat.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'name_rs' });

        var url_kzcover = Source_kzcover.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'GRAY_INDEX' });

        var url_meteo_st = Source_meteo_st.getGetFeatureInfoUrl(evt.coordinate, viewResolution, 'EPSG:3857', { 'INFO_FORMAT': 'text/javascript', 'propertyName': 'wmo_id,name_rs' });
        //---------------------
        var oblasti_value = '';
        var rayony_value = '';

        var analizeterrain_value = 255;

        var srtm_value = -1000;
        var aspect_srtm_value = -1000;
        var slope_srtm_value = -1000;

        lep_gid = [];
        lep_name_rs = [];
        lep_vltg = [];
        lep_rng = [];
        lep_type = [];
        lep_dtntr = [];
        lep_elvt = [];
        lep_mtrl = [];
        lep_insttp = [];
        lep_lnght = [];
        lep_rnzon = [];

        var avg_dnr_year_value = -1;
        var avg_dnr_01_value = -1;
        var avg_dnr_02_value = -1;
        var avg_dnr_03_value = -1;
        var avg_dnr_04_value = -1;
        var avg_dnr_05_value = -1;
        var avg_dnr_06_value = -1;
        var avg_dnr_07_value = -1;
        var avg_dnr_08_value = -1;
        var avg_dnr_09_value = -1;
        var avg_dnr_10_value = -1;
        var avg_dnr_11_value = -1;
        var avg_dnr_12_value = -1;

        var swv_dwn_year_value = -1;
        var swv_dwn_01_value = -1;
        var swv_dwn_02_value = -1;
        var swv_dwn_03_value = -1;
        var swv_dwn_04_value = -1;
        var swv_dwn_05_value = -1;
        var swv_dwn_06_value = -1;
        var swv_dwn_07_value = -1;
        var swv_dwn_08_value = -1;
        var swv_dwn_09_value = -1;
        var swv_dwn_10_value = -1;
        var swv_dwn_11_value = -1;
        var swv_dwn_12_value = -1;

        var exp_dif_year_value = -1;
        var exp_dif_01_value = -1;
        var exp_dif_02_value = -1;
        var exp_dif_03_value = -1;
        var exp_dif_04_value = -1;
        var exp_dif_05_value = -1;
        var exp_dif_06_value = -1;
        var exp_dif_07_value = -1;
        var exp_dif_08_value = -1;
        var exp_dif_09_value = -1;
        var exp_dif_10_value = -1;
        var exp_dif_11_value = -1;
        var exp_dif_12_value = -1;

        var rettlt0opt_year_value = -1;
        var rettlt0opt_01_value = -1;
        var rettlt0opt_02_value = -1;
        var rettlt0opt_03_value = -1;
        var rettlt0opt_04_value = -1;
        var rettlt0opt_05_value = -1;
        var rettlt0opt_06_value = -1;
        var rettlt0opt_07_value = -1;
        var rettlt0opt_08_value = -1;
        var rettlt0opt_09_value = -1;
        var rettlt0opt_10_value = -1;
        var rettlt0opt_11_value = -1;
        var rettlt0opt_12_value = -1;

        var clrskyavrg_year_value = -1;
        var clrskyavrg_01_value = -1;
        var clrskyavrg_02_value = -1;
        var clrskyavrg_03_value = -1;
        var clrskyavrg_04_value = -1;
        var clrskyavrg_05_value = -1;
        var clrskyavrg_06_value = -1;
        var clrskyavrg_07_value = -1;
        var clrskyavrg_08_value = -1;
        var clrskyavrg_09_value = -1;
        var clrskyavrg_10_value = -1;
        var clrskyavrg_11_value = -1;
        var clrskyavrg_12_value = -1;

        var retesh0mim_year_value = -1;
        var retesh0mim_01_value = -1;
        var retesh0mim_02_value = -1;
        var retesh0mim_03_value = -1;
        var retesh0mim_04_value = -1;
        var retesh0mim_05_value = -1;
        var retesh0mim_06_value = -1;
        var retesh0mim_07_value = -1;
        var retesh0mim_08_value = -1;
        var retesh0mim_09_value = -1;
        var retesh0mim_10_value = -1;
        var retesh0mim_11_value = -1;
        var retesh0mim_12_value = -1;

        var dni_year_value = -1;
        var dni_01_value = -1;
        var dni_02_value = -1;
        var dni_03_value = -1;
        var dni_04_value = -1;
        var dni_05_value = -1;
        var dni_06_value = -1;
        var dni_07_value = -1;
        var dni_08_value = -1;
        var dni_09_value = -1;
        var dni_10_value = -1;
        var dni_11_value = -1;
        var dni_12_value = -1;

        var t10m_01_value = -1000;
        var t10m_02_value = -1000;
        var t10m_03_value = -1000;
        var t10m_04_value = -1000;
        var t10m_05_value = -1000;
        var t10m_06_value = -1000;
        var t10m_07_value = -1000;
        var t10m_08_value = -1000;
        var t10m_09_value = -1000;
        var t10m_10_value = -1000;
        var t10m_11_value = -1000;
        var t10m_12_value = -1000;

        var rainavgesm_year_value = -1000;
        var rainavgesm_01_value = -1000;
        var rainavgesm_02_value = -1000;
        var rainavgesm_03_value = -1000;
        var rainavgesm_04_value = -1000;
        var rainavgesm_05_value = -1000;
        var rainavgesm_06_value = -1000;
        var rainavgesm_07_value = -1000;
        var rainavgesm_08_value = -1000;
        var rainavgesm_09_value = -1000;
        var rainavgesm_10_value = -1000;
        var rainavgesm_11_value = -1000;
        var rainavgesm_12_value = -1000;

        var t10mmax_01_value = -1000;
        var t10mmax_02_value = -1000;
        var t10mmax_03_value = -1000;
        var t10mmax_04_value = -1000;
        var t10mmax_05_value = -1000;
        var t10mmax_06_value = -1000;
        var t10mmax_07_value = -1000;
        var t10mmax_08_value = -1000;
        var t10mmax_09_value = -1000;
        var t10mmax_10_value = -1000;
        var t10mmax_11_value = -1000;
        var t10mmax_12_value = -1000;

        var t10m_min_01_value = -1000;
        var t10m_min_02_value = -1000;
        var t10m_min_03_value = -1000;
        var t10m_min_04_value = -1000;
        var t10m_min_05_value = -1000;
        var t10m_min_06_value = -1000;
        var t10m_min_07_value = -1000;
        var t10m_min_08_value = -1000;
        var t10m_min_09_value = -1000;
        var t10m_min_10_value = -1000;
        var t10m_min_11_value = -1000;
        var t10m_min_12_value = -1000;

        var tskinavg_01_value = -1000;
        var tskinavg_02_value = -1000;
        var tskinavg_03_value = -1000;
        var tskinavg_04_value = -1000;
        var tskinavg_05_value = -1000;
        var tskinavg_06_value = -1000;
        var tskinavg_07_value = -1000;
        var tskinavg_08_value = -1000;
        var tskinavg_09_value = -1000;
        var tskinavg_10_value = -1000;
        var tskinavg_11_value = -1000;
        var tskinavg_12_value = -1000;

        var srfalbavg_01_value = -1000;
        var srfalbavg_02_value = -1000;
        var srfalbavg_03_value = -1000;
        var srfalbavg_04_value = -1000;
        var srfalbavg_05_value = -1000;
        var srfalbavg_06_value = -1000;
        var srfalbavg_07_value = -1000;
        var srfalbavg_08_value = -1000;
        var srfalbavg_09_value = -1000;
        var srfalbavg_10_value = -1000;
        var srfalbavg_11_value = -1000;
        var srfalbavg_12_value = -1000;

        var avg_kt_22_01_value = -1000;
        var avg_kt_22_02_value = -1000;
        var avg_kt_22_03_value = -1000;
        var avg_kt_22_04_value = -1000;
        var avg_kt_22_05_value = -1000;
        var avg_kt_22_06_value = -1000;
        var avg_kt_22_07_value = -1000;
        var avg_kt_22_08_value = -1000;
        var avg_kt_22_09_value = -1000;
        var avg_kt_22_10_value = -1000;
        var avg_kt_22_11_value = -1000;
        var avg_kt_22_12_value = -1000;

        var avg_nkt_22_01_value = -1000;
        var avg_nkt_22_02_value = -1000;
        var avg_nkt_22_03_value = -1000;
        var avg_nkt_22_04_value = -1000;
        var avg_nkt_22_05_value = -1000;
        var avg_nkt_22_06_value = -1000;
        var avg_nkt_22_07_value = -1000;
        var avg_nkt_22_08_value = -1000;
        var avg_nkt_22_09_value = -1000;
        var avg_nkt_22_10_value = -1000;
        var avg_nkt_22_11_value = -1000;
        var avg_nkt_22_12_value = -1000;

        var day_cld_22_01_value = -1000;
        var day_cld_22_02_value = -1000;
        var day_cld_22_03_value = -1000;
        var day_cld_22_04_value = -1000;
        var day_cld_22_05_value = -1000;
        var day_cld_22_06_value = -1000;
        var day_cld_22_07_value = -1000;
        var day_cld_22_08_value = -1000;
        var day_cld_22_09_value = -1000;
        var day_cld_22_10_value = -1000;
        var day_cld_22_11_value = -1000;
        var day_cld_22_12_value = -1000;

        var daylghtav_01_value = -1000;
        var daylghtav_02_value = -1000;
        var daylghtav_03_value = -1000;
        var daylghtav_04_value = -1000;
        var daylghtav_05_value = -1000;
        var daylghtav_06_value = -1000;
        var daylghtav_07_value = -1000;
        var daylghtav_08_value = -1000;
        var daylghtav_09_value = -1000;
        var daylghtav_10_value = -1000;
        var daylghtav_11_value = -1000;
        var daylghtav_12_value = -1000;

        var p_clr_cky_year_value = -1;
        var p_clr_cky_01_value = -1;
        var p_clr_cky_02_value = -1;
        var p_clr_cky_03_value = -1;
        var p_clr_cky_04_value = -1;
        var p_clr_cky_05_value = -1;
        var p_clr_cky_06_value = -1;
        var p_clr_cky_07_value = -1;
        var p_clr_cky_08_value = -1;
        var p_clr_cky_09_value = -1;
        var p_clr_cky_10_value = -1;
        var p_clr_cky_11_value = -1;
        var p_clr_cky_12_value = -1;

        var p_swv_dwn_year_value = -1;
        var p_swv_dwn_01_value = -1;
        var p_swv_dwn_02_value = -1;
        var p_swv_dwn_03_value = -1;
        var p_swv_dwn_04_value = -1;
        var p_swv_dwn_05_value = -1;
        var p_swv_dwn_06_value = -1;
        var p_swv_dwn_07_value = -1;
        var p_swv_dwn_08_value = -1;
        var p_swv_dwn_09_value = -1;
        var p_swv_dwn_10_value = -1;
        var p_swv_dwn_11_value = -1;
        var p_swv_dwn_12_value = -1;

        var p_toa_dwn_year_value = -1;
        var p_toa_dwn_01_value = -1;
        var p_toa_dwn_02_value = -1;
        var p_toa_dwn_03_value = -1;
        var p_toa_dwn_04_value = -1;
        var p_toa_dwn_05_value = -1;
        var p_toa_dwn_06_value = -1;
        var p_toa_dwn_07_value = -1;
        var p_toa_dwn_08_value = -1;
        var p_toa_dwn_09_value = -1;
        var p_toa_dwn_10_value = -1;
        var p_toa_dwn_11_value = -1;
        var p_toa_dwn_12_value = -1;

        var dni_year_value = -1;
        var dni_01_value = -1;
        var dni_02_value = -1;
        var dni_03_value = -1;
        var dni_04_value = -1;
        var dni_05_value = -1;
        var dni_06_value = -1;
        var dni_07_value = -1;
        var dni_08_value = -1;
        var dni_09_value = -1;
        var dni_10_value = -1;
        var dni_11_value = -1;
        var dni_12_value = -1;

        var sic_year_value = -1;
        var sic_01_value = -1;
        var sic_02_value = -1;
        var sic_03_value = -1;
        var sic_04_value = -1;
        var sic_05_value = -1;
        var sic_06_value = -1;
        var sic_07_value = -1;
        var sic_08_value = -1;
        var sic_09_value = -1;
        var sic_10_value = -1;
        var sic_11_value = -1;
        var sic_12_value = -1;

        var sid_year_value = -1;
        var sid_01_value = -1;
        var sid_02_value = -1;
        var sid_03_value = -1;
        var sid_04_value = -1;
        var sid_05_value = -1;
        var sid_06_value = -1;
        var sid_07_value = -1;
        var sid_08_value = -1;
        var sid_09_value = -1;
        var sid_10_value = -1;
        var sid_11_value = -1;
        var sid_12_value = -1;

        var sis_year_value = -1;
        var sis_01_value = -1;
        var sis_02_value = -1;
        var sis_03_value = -1;
        var sis_04_value = -1;
        var sis_05_value = -1;
        var sis_06_value = -1;
        var sis_07_value = -1;
        var sis_08_value = -1;
        var sis_09_value = -1;
        var sis_10_value = -1;
        var sis_11_value = -1;
        var sis_12_value = -1;

        var sis_klr_year_value = -1;
        var sis_klr_01_value = -1;
        var sis_klr_02_value = -1;
        var sis_klr_03_value = -1;
        var sis_klr_04_value = -1;
        var sis_klr_05_value = -1;
        var sis_klr_06_value = -1;
        var sis_klr_07_value = -1;
        var sis_klr_08_value = -1;
        var sis_klr_09_value = -1;
        var sis_klr_10_value = -1;
        var sis_klr_11_value = -1;
        var sis_klr_12_value = -1;

        var pamyatnikprirodypol_value = '';
        var prirparki_value = '';
        var rezervaty_value = '';
        var zakazniky_value = '';
        var zapovedniki_value = '';
        var zapovedzony_value = '';

        var hidroohrzony_value = '';
        var arheopamyat_value = '';

        var kzcover_value = 255;

        var OPTANG_value = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

        var meteo_st_name_rs = '';
        var meteo_st_wmo_id = -1;
        //---------------------
        function Get_oblasti() {
            content = "";
            content += '<tr><td colspan="2" style="text-align: center;">' + $('#RetrievingData').html() + '</td>'
                    //+ '<td style="padding-left: 10px;"></td>'
                    + '</tr>';
            $('#dialog_table').append(content);
            jQuery.ajax({
                jsonp: false,
                jsonpCallback: 'getJson',
                type: 'GET',
                url: url_oblasti + "&format_options=callback:getJson",
                async: false,
                dataType: 'jsonp',
                error: function () {
                }
            }).then(function (data_oblasti) {
                if (data_oblasti.features.length > 0) {
                    if ($('#language').html() == 'en') {
                        oblasti_value = data_oblasti.features[0].properties.name_an;
                    }
                    if ($('#language').html() == 'kk') {
                        oblasti_value = data_oblasti.features[0].properties.name_kz;
                    }
                    if ($('#language').html() == 'ru') {
                        oblasti_value = data_oblasti.features[0].properties.name_rs;
                    }
                    Get_rayony();
                }
                else
                {
                    oblasti_value = '';
                    Get_zapovedzony();
                }
            });
        };
        //---------------------
        function Get_rayony() {
            jQuery.ajax({
                jsonp: false,
                jsonpCallback: 'getJson',
                type: 'GET',
                url: url_rayony + "&format_options=callback:getJson",
                async: false,
                dataType: 'jsonp',
                error: function () {
                }
            }).then(function (data_rayony) {
                if ($('#language').html() == 'en') {
                    rayony_value = data_rayony.features[0].properties.name_an;
                }
                if ($('#language').html() == 'kk') {
                    rayony_value = data_rayony.features[0].properties.name_kz;
                }
                if ($('#language').html() == 'ru') {
                    rayony_value = data_rayony.features[0].properties.name_rs;
                }
                Get_analizeterrain();
            });
        };
        //---------------------
        function Get_analizeterrain() {
            if (Layer_AnalizeTerrain.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_analizeterrain + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_analizeterrain) {
                    if (data_analizeterrain.features.length > 0) {
                        analizeterrain_value = data_analizeterrain.features[0].properties.GRAY_INDEX;
                    }
                    else
                    {
                        analizeterrain_value = 255;
                    }
                    Get_lep();
                });
            }
            else {
                Get_lep();
            }
        };
        //---------------------
        function Get_lep() {
            if (Layer_lep.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_lep + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_lep) {
                    if (data_lep.features.length > 0) {
                        for (i = 0; i < data_lep.features.length; i++) {
                            lep_gid.push(data_lep.features[i].properties.gid);
                            lep_name_rs.push(data_lep.features[i].properties.name_rs);
                            lep_rng.push(data_lep.features[i].properties.rng);
                            lep_vltg.push(data_lep.features[i].properties.vltg);
                            lep_type.push(data_lep.features[i].properties.type);
                            lep_dtntr.push(data_lep.features[i].properties.dtntr);
                            lep_elvt.push(data_lep.features[i].properties.elvt);
                            lep_mtrl.push(data_lep.features[i].properties.mtrl);
                            lep_insttp.push(data_lep.features[i].properties.insttp);
                            lep_lnght.push(data_lep.features[i].properties.lnght);
                            lep_rnzon.push(data_lep.features[i].properties.rnzon);
                            var lineFeature = new ol.Feature({
                                geometry: new ol.geom.LineString(data_lep.features[i].geometry.coordinates[0])
                            });
                            Source_select.addFeature(lineFeature);
                        }
                    }
                    Get_srtm();
                });
            }
            else {
                Get_srtm();
            }
        };
        //---------------------
        function Get_srtm() {
            if (Layer_srtm.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_srtm + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_srtm) {
                    srtm_value = data_srtm.features[0].properties.GRAY_INDEX;
                    Get_aspect_srtm();
                });
            }
            else {
                Get_aspect_srtm();
            }
        };
        //---------------------
        function Get_aspect_srtm() {
            if (Layer_aspect_srtm.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_aspect_srtm + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_aspect_srtm) {
                    aspect_srtm_value = data_aspect_srtm.features[0].properties.GRAY_INDEX;
                    Get_slope_srtm();
                });
            }
            else {
                Get_slope_srtm();
            }
        };
        //---------------------
        function Get_slope_srtm() {
            if (Layer_slope_srtm.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_slope_srtm + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_slope_srtm) {
                    slope_srtm_value = data_slope_srtm.features[0].properties.GRAY_INDEX;
                    Get_avg_dnr_year();
                });
            }
            else {
                Get_avg_dnr_year();
            }
        };
        //---------------------
        function Get_avg_dnr_year() {
            if (Layer_avg_dnr_year.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_dnr_year + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_dnr_year) {
                    avg_dnr_year_value = data_avg_dnr_year.features[0].properties.GRAY_INDEX;
                    Get_avg_dnr_01();
                });
            }
            else {
                Get_avg_dnr_01();
            }
        };
        //---------------------
        function Get_avg_dnr_01() {
            if (Layer_avg_dnr_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_dnr_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_dnr_01) {
                    avg_dnr_01_value = data_avg_dnr_01.features[0].properties.GRAY_INDEX;
                    Get_avg_dnr_02();
                });
            }
            else {
                Get_avg_dnr_02();
            }
        };
        //---------------------
        function Get_avg_dnr_02() {
            if (Layer_avg_dnr_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_dnr_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_dnr_02) {
                    avg_dnr_02_value = data_avg_dnr_02.features[0].properties.GRAY_INDEX;
                    Get_avg_dnr_03();
                });
            }
            else {
                Get_avg_dnr_03();
            }
        };
        //---------------------
        function Get_avg_dnr_03() {
            if (Layer_avg_dnr_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_dnr_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_dnr_03) {
                    avg_dnr_03_value = data_avg_dnr_03.features[0].properties.GRAY_INDEX;
                    Get_avg_dnr_04();
                });
            }
            else {
                Get_avg_dnr_04();
            }
        };
        //---------------------
        function Get_avg_dnr_04() {
            if (Layer_avg_dnr_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_dnr_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_dnr_04) {
                    avg_dnr_04_value = data_avg_dnr_04.features[0].properties.GRAY_INDEX;
                    Get_avg_dnr_05();
                });
            }
            else {
                Get_avg_dnr_05();
            }
        };
        //---------------------
        function Get_avg_dnr_05() {
            if (Layer_avg_dnr_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_dnr_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_dnr_05) {
                    avg_dnr_05_value = data_avg_dnr_05.features[0].properties.GRAY_INDEX;
                    Get_avg_dnr_06();
                });
            }
            else {
                Get_avg_dnr_06();
            }
        };
        //---------------------
        function Get_avg_dnr_06() {
            if (Layer_avg_dnr_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_dnr_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_dnr_06) {
                    avg_dnr_06_value = data_avg_dnr_06.features[0].properties.GRAY_INDEX;
                    Get_avg_dnr_07();
                });
            }
            else {
                Get_avg_dnr_07();
            }
        };
        //---------------------
        function Get_avg_dnr_07() {
            if (Layer_avg_dnr_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_dnr_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_dnr_07) {
                    avg_dnr_07_value = data_avg_dnr_07.features[0].properties.GRAY_INDEX;
                    Get_avg_dnr_08();
                });
            }
            else {
                Get_avg_dnr_08();
            }
        };
        //---------------------
        function Get_avg_dnr_08() {
            if (Layer_avg_dnr_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_dnr_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_dnr_08) {
                    avg_dnr_08_value = data_avg_dnr_08.features[0].properties.GRAY_INDEX;
                    Get_avg_dnr_09();
                });
            }
            else {
                Get_avg_dnr_09();
            }
        };
        //---------------------
        function Get_avg_dnr_09() {
            if (Layer_avg_dnr_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_dnr_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_dnr_09) {
                    avg_dnr_09_value = data_avg_dnr_09.features[0].properties.GRAY_INDEX;
                    Get_avg_dnr_10();
                });
            }
            else {
                Get_avg_dnr_10();
            }
        };
        //---------------------
        function Get_avg_dnr_10() {
            if (Layer_avg_dnr_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_dnr_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_dnr_10) {
                    avg_dnr_10_value = data_avg_dnr_10.features[0].properties.GRAY_INDEX;
                    Get_avg_dnr_11();
                });
            }
            else {
                Get_avg_dnr_11();
            }
        };
        //---------------------
        function Get_avg_dnr_11() {
            if (Layer_avg_dnr_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_dnr_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_dnr_11) {
                    avg_dnr_11_value = data_avg_dnr_11.features[0].properties.GRAY_INDEX;
                    Get_avg_dnr_12();
                });
            }
            else {
                Get_avg_dnr_12();
            }
        };
        //---------------------
        function Get_avg_dnr_12() {
            if (Layer_avg_dnr_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_dnr_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_dnr_12) {
                    avg_dnr_12_value = data_avg_dnr_12.features[0].properties.GRAY_INDEX;
                    Get_swv_dwn_year();
                });
            }
            else {
                Get_swv_dwn_year();
            }
        };
        //---------------------
        function Get_swv_dwn_year() {
            if (Layer_swv_dwn_year.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_swv_dwn_year + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_swv_dwn_year) {
                    swv_dwn_year_value = data_swv_dwn_year.features[0].properties.GRAY_INDEX;
                    Get_swv_dwn_01();
                });
            }
            else {
                Get_swv_dwn_01();
            }
        };
        //---------------------
        function Get_swv_dwn_01() {
            if (Layer_swv_dwn_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_swv_dwn_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_swv_dwn_01) {
                    swv_dwn_01_value = data_swv_dwn_01.features[0].properties.GRAY_INDEX;
                    Get_swv_dwn_02();
                });
            }
            else {
                Get_swv_dwn_02();
            }
        };
        //---------------------
        function Get_swv_dwn_02() {
            if (Layer_swv_dwn_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_swv_dwn_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_swv_dwn_02) {
                    swv_dwn_02_value = data_swv_dwn_02.features[0].properties.GRAY_INDEX;
                    Get_swv_dwn_03();
                });
            }
            else {
                Get_swv_dwn_03();
            }
        };
        //---------------------
        function Get_swv_dwn_03() {
            if (Layer_swv_dwn_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_swv_dwn_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_swv_dwn_03) {
                    swv_dwn_03_value = data_swv_dwn_03.features[0].properties.GRAY_INDEX;
                    Get_swv_dwn_04();
                });
            }
            else {
                Get_swv_dwn_04();
            }
        };
        //---------------------
        function Get_swv_dwn_04() {
            if (Layer_swv_dwn_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_swv_dwn_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_swv_dwn_04) {
                    swv_dwn_04_value = data_swv_dwn_04.features[0].properties.GRAY_INDEX;
                    Get_swv_dwn_05();
                });
            }
            else {
                Get_swv_dwn_05();
            }
        };
        //---------------------
        function Get_swv_dwn_05() {
            if (Layer_swv_dwn_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_swv_dwn_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_swv_dwn_05) {
                    swv_dwn_05_value = data_swv_dwn_05.features[0].properties.GRAY_INDEX;
                    Get_swv_dwn_06();
                });
            }
            else {
                Get_swv_dwn_06();
            }
        };
        //---------------------
        function Get_swv_dwn_06() {
            if (Layer_swv_dwn_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_swv_dwn_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_swv_dwn_06) {
                    swv_dwn_06_value = data_swv_dwn_06.features[0].properties.GRAY_INDEX;
                    Get_swv_dwn_07();
                });
            }
            else {
                Get_swv_dwn_07();
            }
        };
        //---------------------
        function Get_swv_dwn_07() {
            if (Layer_swv_dwn_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_swv_dwn_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_swv_dwn_07) {
                    swv_dwn_07_value = data_swv_dwn_07.features[0].properties.GRAY_INDEX;
                    Get_swv_dwn_08();
                });
            }
            else {
                Get_swv_dwn_08();
            }
        };
        //---------------------
        function Get_swv_dwn_08() {
            if (Layer_swv_dwn_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_swv_dwn_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_swv_dwn_08) {
                    swv_dwn_08_value = data_swv_dwn_08.features[0].properties.GRAY_INDEX;
                    Get_swv_dwn_09();
                });
            }
            else {
                Get_swv_dwn_09();
            }
        };
        //---------------------
        function Get_swv_dwn_09() {
            if (Layer_swv_dwn_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_swv_dwn_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_swv_dwn_09) {
                    swv_dwn_09_value = data_swv_dwn_09.features[0].properties.GRAY_INDEX;
                    Get_swv_dwn_10();
                });
            }
            else {
                Get_swv_dwn_10();
            }
        };
        //---------------------
        function Get_swv_dwn_10() {
            if (Layer_swv_dwn_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_swv_dwn_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_swv_dwn_10) {
                    swv_dwn_10_value = data_swv_dwn_10.features[0].properties.GRAY_INDEX;
                    Get_swv_dwn_11();
                });
            }
            else {
                Get_swv_dwn_11();
            }
        };
        //---------------------
        function Get_swv_dwn_11() {
            if (Layer_swv_dwn_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_swv_dwn_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_swv_dwn_11) {
                    swv_dwn_11_value = data_swv_dwn_11.features[0].properties.GRAY_INDEX;
                    Get_swv_dwn_12();
                });
            }
            else {
                Get_swv_dwn_12();
            }
        };
        //---------------------
        function Get_swv_dwn_12() {
            if (Layer_swv_dwn_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_swv_dwn_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_swv_dwn_12) {
                    swv_dwn_12_value = data_swv_dwn_12.features[0].properties.GRAY_INDEX;
                    Get_exp_dif_year();
                });
            }
            else {
                Get_exp_dif_year();
            }
        };
        //---------------------
        function Get_exp_dif_year() {
            if (Layer_exp_dif_year.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_exp_dif_year + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_exp_dif_year) {
                    exp_dif_year_value = data_exp_dif_year.features[0].properties.GRAY_INDEX;
                    Get_exp_dif_01();
                });
            }
            else {
                Get_exp_dif_01();
            }
        };
        //---------------------
        function Get_exp_dif_01() {
            if (Layer_exp_dif_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_exp_dif_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_exp_dif_01) {
                    exp_dif_01_value = data_exp_dif_01.features[0].properties.GRAY_INDEX;
                    Get_exp_dif_02();
                });
            }
            else {
                Get_exp_dif_02();
            }
        };
        //---------------------
        function Get_exp_dif_02() {
            if (Layer_exp_dif_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_exp_dif_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_exp_dif_02) {
                    exp_dif_02_value = data_exp_dif_02.features[0].properties.GRAY_INDEX;
                    Get_exp_dif_03();
                });
            }
            else {
                Get_exp_dif_03();
            }
        };
        //---------------------
        function Get_exp_dif_03() {
            if (Layer_exp_dif_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_exp_dif_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_exp_dif_03) {
                    exp_dif_03_value = data_exp_dif_03.features[0].properties.GRAY_INDEX;
                    Get_exp_dif_04();
                });
            }
            else {
                Get_exp_dif_04();
            }
        };
        //---------------------
        function Get_exp_dif_04() {
            if (Layer_exp_dif_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_exp_dif_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_exp_dif_04) {
                    exp_dif_04_value = data_exp_dif_04.features[0].properties.GRAY_INDEX;
                    Get_exp_dif_05();
                });
            }
            else {
                Get_exp_dif_05();
            }
        };
        //---------------------
        function Get_exp_dif_05() {
            if (Layer_exp_dif_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_exp_dif_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_exp_dif_05) {
                    exp_dif_05_value = data_exp_dif_05.features[0].properties.GRAY_INDEX;
                    Get_exp_dif_06();
                });
            }
            else {
                Get_exp_dif_06();
            }
        };
        //---------------------
        function Get_exp_dif_06() {
            if (Layer_exp_dif_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_exp_dif_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_exp_dif_06) {
                    exp_dif_06_value = data_exp_dif_06.features[0].properties.GRAY_INDEX;
                    Get_exp_dif_07();
                });
            }
            else {
                Get_exp_dif_07();
            }
        };
        //---------------------
        function Get_exp_dif_07() {
            if (Layer_exp_dif_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_exp_dif_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_exp_dif_07) {
                    exp_dif_07_value = data_exp_dif_07.features[0].properties.GRAY_INDEX;
                    Get_exp_dif_08();
                });
            }
            else {
                Get_exp_dif_08();
            }
        };
        //---------------------
        function Get_exp_dif_08() {
            if (Layer_exp_dif_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_exp_dif_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_exp_dif_08) {
                    exp_dif_08_value = data_exp_dif_08.features[0].properties.GRAY_INDEX;
                    Get_exp_dif_09();
                });
            }
            else {
                Get_exp_dif_09();
            }
        };
        //---------------------
        function Get_exp_dif_09() {
            if (Layer_exp_dif_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_exp_dif_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_exp_dif_09) {
                    exp_dif_09_value = data_exp_dif_09.features[0].properties.GRAY_INDEX;
                    Get_exp_dif_10();
                });
            }
            else {
                Get_exp_dif_10();
            }
        };
        //---------------------
        function Get_exp_dif_10() {
            if (Layer_exp_dif_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_exp_dif_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_exp_dif_10) {
                    exp_dif_10_value = data_exp_dif_10.features[0].properties.GRAY_INDEX;
                    Get_exp_dif_11();
                });
            }
            else {
                Get_exp_dif_11();
            }
        };
        //---------------------
        function Get_exp_dif_11() {
            if (Layer_exp_dif_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_exp_dif_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_exp_dif_11) {
                    exp_dif_11_value = data_exp_dif_11.features[0].properties.GRAY_INDEX;
                    Get_exp_dif_12();
                });
            }
            else {
                Get_exp_dif_12();
            }
        };
        //---------------------
        function Get_exp_dif_12() {
            if (Layer_exp_dif_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_exp_dif_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_exp_dif_12) {
                    exp_dif_12_value = data_exp_dif_12.features[0].properties.GRAY_INDEX;
                    Get_rettlt0opt_year();
                });
            }
            else {
                Get_rettlt0opt_year();
            }
        };

        //---------------------
        function Get_rettlt0opt_year() {
            if (Layer_rettlt0opt_year.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rettlt0opt_year + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rettlt0opt_year) {
                    rettlt0opt_year_value = data_rettlt0opt_year.features[0].properties.GRAY_INDEX;
                    Get_rettlt0opt_01();
                });
            }
            else {
                Get_rettlt0opt_01();
            }
        };
        //---------------------
        function Get_rettlt0opt_01() {
            if (Layer_rettlt0opt_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rettlt0opt_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rettlt0opt_01) {
                    rettlt0opt_01_value = data_rettlt0opt_01.features[0].properties.GRAY_INDEX;
                    Get_rettlt0opt_02();
                });
            }
            else {
                Get_rettlt0opt_02();
            }
        };
        //---------------------
        function Get_rettlt0opt_02() {
            if (Layer_rettlt0opt_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rettlt0opt_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rettlt0opt_02) {
                    rettlt0opt_02_value = data_rettlt0opt_02.features[0].properties.GRAY_INDEX;
                    Get_rettlt0opt_03();
                });
            }
            else {
                Get_rettlt0opt_03();
            }
        };
        //---------------------
        function Get_rettlt0opt_03() {
            if (Layer_rettlt0opt_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rettlt0opt_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rettlt0opt_03) {
                    rettlt0opt_03_value = data_rettlt0opt_03.features[0].properties.GRAY_INDEX;
                    Get_rettlt0opt_04();
                });
            }
            else {
                Get_rettlt0opt_04();
            }
        };
        //---------------------
        function Get_rettlt0opt_04() {
            if (Layer_rettlt0opt_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rettlt0opt_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rettlt0opt_04) {
                    rettlt0opt_04_value = data_rettlt0opt_04.features[0].properties.GRAY_INDEX;
                    Get_rettlt0opt_05();
                });
            }
            else {
                Get_rettlt0opt_05();
            }
        };
        //---------------------
        function Get_rettlt0opt_05() {
            if (Layer_rettlt0opt_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rettlt0opt_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rettlt0opt_05) {
                    rettlt0opt_05_value = data_rettlt0opt_05.features[0].properties.GRAY_INDEX;
                    Get_rettlt0opt_06();
                });
            }
            else {
                Get_rettlt0opt_06();
            }
        };
        //---------------------
        function Get_rettlt0opt_06() {
            if (Layer_rettlt0opt_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rettlt0opt_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rettlt0opt_06) {
                    rettlt0opt_06_value = data_rettlt0opt_06.features[0].properties.GRAY_INDEX;
                    Get_rettlt0opt_07();
                });
            }
            else {
                Get_rettlt0opt_07();
            }
        };
        //---------------------
        function Get_rettlt0opt_07() {
            if (Layer_rettlt0opt_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rettlt0opt_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rettlt0opt_07) {
                    rettlt0opt_07_value = data_rettlt0opt_07.features[0].properties.GRAY_INDEX;
                    Get_rettlt0opt_08();
                });
            }
            else {
                Get_rettlt0opt_08();
            }
        };
        //---------------------
        function Get_rettlt0opt_08() {
            if (Layer_rettlt0opt_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rettlt0opt_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rettlt0opt_08) {
                    rettlt0opt_08_value = data_rettlt0opt_08.features[0].properties.GRAY_INDEX;
                    Get_rettlt0opt_09();
                });
            }
            else {
                Get_rettlt0opt_09();
            }
        };
        //---------------------
        function Get_rettlt0opt_09() {
            if (Layer_rettlt0opt_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rettlt0opt_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rettlt0opt_09) {
                    rettlt0opt_09_value = data_rettlt0opt_09.features[0].properties.GRAY_INDEX;
                    Get_rettlt0opt_10();
                });
            }
            else {
                Get_rettlt0opt_10();
            }
        };
        //---------------------
        function Get_rettlt0opt_10() {
            if (Layer_rettlt0opt_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rettlt0opt_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rettlt0opt_10) {
                    rettlt0opt_10_value = data_rettlt0opt_10.features[0].properties.GRAY_INDEX;
                    Get_rettlt0opt_11();
                });
            }
            else {
                Get_rettlt0opt_11();
            }
        };
        //---------------------
        function Get_rettlt0opt_11() {
            if (Layer_rettlt0opt_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rettlt0opt_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rettlt0opt_11) {
                    rettlt0opt_11_value = data_rettlt0opt_11.features[0].properties.GRAY_INDEX;
                    Get_rettlt0opt_12();
                });
            }
            else {
                Get_rettlt0opt_12();
            }
        };
        //---------------------
        function Get_rettlt0opt_12() {
            if (Layer_rettlt0opt_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rettlt0opt_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rettlt0opt_12) {
                    rettlt0opt_12_value = data_rettlt0opt_12.features[0].properties.GRAY_INDEX;
                    Get_clrskyavrg_year();
                });
            }
            else {
                Get_clrskyavrg_year();
            }
        };

        //---------------------
        function Get_clrskyavrg_year() {
            if (Layer_clrskyavrg_year.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_clrskyavrg_year + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_clrskyavrg_year) {
                    clrskyavrg_year_value = data_clrskyavrg_year.features[0].properties.GRAY_INDEX;
                    Get_clrskyavrg_01();
                });
            }
            else {
                Get_clrskyavrg_01();
            }
        };
        //---------------------
        function Get_clrskyavrg_01() {
            if (Layer_clrskyavrg_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_clrskyavrg_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_clrskyavrg_01) {
                    clrskyavrg_01_value = data_clrskyavrg_01.features[0].properties.GRAY_INDEX;
                    Get_clrskyavrg_02();
                });
            }
            else {
                Get_clrskyavrg_02();
            }
        };
        //---------------------
        function Get_clrskyavrg_02() {
            if (Layer_clrskyavrg_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_clrskyavrg_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_clrskyavrg_02) {
                    clrskyavrg_02_value = data_clrskyavrg_02.features[0].properties.GRAY_INDEX;
                    Get_clrskyavrg_03();
                });
            }
            else {
                Get_clrskyavrg_03();
            }
        };
        //---------------------
        function Get_clrskyavrg_03() {
            if (Layer_clrskyavrg_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_clrskyavrg_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_clrskyavrg_03) {
                    clrskyavrg_03_value = data_clrskyavrg_03.features[0].properties.GRAY_INDEX;
                    Get_clrskyavrg_04();
                });
            }
            else {
                Get_clrskyavrg_04();
            }
        };
        //---------------------
        function Get_clrskyavrg_04() {
            if (Layer_clrskyavrg_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_clrskyavrg_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_clrskyavrg_04) {
                    clrskyavrg_04_value = data_clrskyavrg_04.features[0].properties.GRAY_INDEX;
                    Get_clrskyavrg_05();
                });
            }
            else {
                Get_clrskyavrg_05();
            }
        };
        //---------------------
        function Get_clrskyavrg_05() {
            if (Layer_clrskyavrg_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_clrskyavrg_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_clrskyavrg_05) {
                    clrskyavrg_05_value = data_clrskyavrg_05.features[0].properties.GRAY_INDEX;
                    Get_clrskyavrg_06();
                });
            }
            else {
                Get_clrskyavrg_06();
            }
        };
        //---------------------
        function Get_clrskyavrg_06() {
            if (Layer_clrskyavrg_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_clrskyavrg_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_clrskyavrg_06) {
                    clrskyavrg_06_value = data_clrskyavrg_06.features[0].properties.GRAY_INDEX;
                    Get_clrskyavrg_07();
                });
            }
            else {
                Get_clrskyavrg_07();
            }
        };
        //---------------------
        function Get_clrskyavrg_07() {
            if (Layer_clrskyavrg_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_clrskyavrg_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_clrskyavrg_07) {
                    clrskyavrg_07_value = data_clrskyavrg_07.features[0].properties.GRAY_INDEX;
                    Get_clrskyavrg_08();
                });
            }
            else {
                Get_clrskyavrg_08();
            }
        };
        //---------------------
        function Get_clrskyavrg_08() {
            if (Layer_clrskyavrg_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_clrskyavrg_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_clrskyavrg_08) {
                    clrskyavrg_08_value = data_clrskyavrg_08.features[0].properties.GRAY_INDEX;
                    Get_clrskyavrg_09();
                });
            }
            else {
                Get_clrskyavrg_09();
            }
        };
        //---------------------
        function Get_clrskyavrg_09() {
            if (Layer_clrskyavrg_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_clrskyavrg_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_clrskyavrg_09) {
                    clrskyavrg_09_value = data_clrskyavrg_09.features[0].properties.GRAY_INDEX;
                    Get_clrskyavrg_10();
                });
            }
            else {
                Get_clrskyavrg_10();
            }
        };
        //---------------------
        function Get_clrskyavrg_10() {
            if (Layer_clrskyavrg_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_clrskyavrg_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_clrskyavrg_10) {
                    clrskyavrg_10_value = data_clrskyavrg_10.features[0].properties.GRAY_INDEX;
                    Get_clrskyavrg_11();
                });
            }
            else {
                Get_clrskyavrg_11();
            }
        };
        //---------------------
        function Get_clrskyavrg_11() {
            if (Layer_clrskyavrg_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_clrskyavrg_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_clrskyavrg_11) {
                    clrskyavrg_11_value = data_clrskyavrg_11.features[0].properties.GRAY_INDEX;
                    Get_clrskyavrg_12();
                });
            }
            else {
                Get_clrskyavrg_12();
            }
        };
        //---------------------
        function Get_clrskyavrg_12() {
            if (Layer_clrskyavrg_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_clrskyavrg_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_clrskyavrg_12) {
                    clrskyavrg_12_value = data_clrskyavrg_12.features[0].properties.GRAY_INDEX;
                    Get_retesh0mim_year();
                });
            }
            else {
                Get_retesh0mim_year();
            }
        };

        //---------------------
        function Get_retesh0mim_year() {
            if (Layer_retesh0mim_year.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_retesh0mim_year + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_retesh0mim_year) {
                    retesh0mim_year_value = data_retesh0mim_year.features[0].properties.GRAY_INDEX;
                    Get_retesh0mim_01();
                });
            }
            else {
                Get_retesh0mim_01();
            }
        };
        //---------------------
        function Get_retesh0mim_01() {
            if (Layer_retesh0mim_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_retesh0mim_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_retesh0mim_01) {
                    retesh0mim_01_value = data_retesh0mim_01.features[0].properties.GRAY_INDEX;
                    Get_retesh0mim_02();
                });
            }
            else {
                Get_retesh0mim_02();
            }
        };
        //---------------------
        function Get_retesh0mim_02() {
            if (Layer_retesh0mim_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_retesh0mim_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_retesh0mim_02) {
                    retesh0mim_02_value = data_retesh0mim_02.features[0].properties.GRAY_INDEX;
                    Get_retesh0mim_03();
                });
            }
            else {
                Get_retesh0mim_03();
            }
        };
        //---------------------
        function Get_retesh0mim_03() {
            if (Layer_retesh0mim_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_retesh0mim_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_retesh0mim_03) {
                    retesh0mim_03_value = data_retesh0mim_03.features[0].properties.GRAY_INDEX;
                    Get_retesh0mim_04();
                });
            }
            else {
                Get_retesh0mim_04();
            }
        };
        //---------------------
        function Get_retesh0mim_04() {
            if (Layer_retesh0mim_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_retesh0mim_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_retesh0mim_04) {
                    retesh0mim_04_value = data_retesh0mim_04.features[0].properties.GRAY_INDEX;
                    Get_retesh0mim_05();
                });
            }
            else {
                Get_retesh0mim_05();
            }
        };
        //---------------------
        function Get_retesh0mim_05() {
            if (Layer_retesh0mim_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_retesh0mim_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_retesh0mim_05) {
                    retesh0mim_05_value = data_retesh0mim_05.features[0].properties.GRAY_INDEX;
                    Get_retesh0mim_06();
                });
            }
            else {
                Get_retesh0mim_06();
            }
        };
        //---------------------
        function Get_retesh0mim_06() {
            if (Layer_retesh0mim_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_retesh0mim_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_retesh0mim_06) {
                    retesh0mim_06_value = data_retesh0mim_06.features[0].properties.GRAY_INDEX;
                    Get_retesh0mim_07();
                });
            }
            else {
                Get_retesh0mim_07();
            }
        };
        //---------------------
        function Get_retesh0mim_07() {
            if (Layer_retesh0mim_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_retesh0mim_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_retesh0mim_07) {
                    retesh0mim_07_value = data_retesh0mim_07.features[0].properties.GRAY_INDEX;
                    Get_retesh0mim_08();
                });
            }
            else {
                Get_retesh0mim_08();
            }
        };
        //---------------------
        function Get_retesh0mim_08() {
            if (Layer_retesh0mim_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_retesh0mim_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_retesh0mim_08) {
                    retesh0mim_08_value = data_retesh0mim_08.features[0].properties.GRAY_INDEX;
                    Get_retesh0mim_09();
                });
            }
            else {
                Get_retesh0mim_09();
            }
        };
        //---------------------
        function Get_retesh0mim_09() {
            if (Layer_retesh0mim_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_retesh0mim_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_retesh0mim_09) {
                    retesh0mim_09_value = data_retesh0mim_09.features[0].properties.GRAY_INDEX;
                    Get_retesh0mim_10();
                });
            }
            else {
                Get_retesh0mim_10();
            }
        };
        //---------------------
        function Get_retesh0mim_10() {
            if (Layer_retesh0mim_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_retesh0mim_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_retesh0mim_10) {
                    retesh0mim_10_value = data_retesh0mim_10.features[0].properties.GRAY_INDEX;
                    Get_retesh0mim_11();
                });
            }
            else {
                Get_retesh0mim_11();
            }
        };
        //---------------------
        function Get_retesh0mim_11() {
            if (Layer_retesh0mim_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_retesh0mim_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_retesh0mim_11) {
                    retesh0mim_11_value = data_retesh0mim_11.features[0].properties.GRAY_INDEX;
                    Get_retesh0mim_12();
                });
            }
            else {
                Get_retesh0mim_12();
            }
        };
        //---------------------
        function Get_retesh0mim_12() {
            if (Layer_retesh0mim_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_retesh0mim_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_retesh0mim_12) {
                    retesh0mim_12_value = data_retesh0mim_12.features[0].properties.GRAY_INDEX;
                    Get_t10m_01();
                });
            }
            else {
                Get_t10m_01();
            }
        };
        //---------------------
        function Get_t10m_01() {
            if (Layer_t10m_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_01) {
                    t10m_01_value = data_t10m_01.features[0].properties.GRAY_INDEX;
                    Get_t10m_02();
                });
            }
            else {
                Get_t10m_02();
            }
        };
        //---------------------
        function Get_t10m_02() {
            if (Layer_t10m_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_02) {
                    t10m_02_value = data_t10m_02.features[0].properties.GRAY_INDEX;
                    Get_t10m_03();
                });
            }
            else {
                Get_t10m_03();
            }
        };
        //---------------------
        function Get_t10m_03() {
            if (Layer_t10m_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_03) {
                    t10m_03_value = data_t10m_03.features[0].properties.GRAY_INDEX;
                    Get_t10m_04();
                });
            }
            else {
                Get_t10m_04();
            }
        };
        //---------------------
        function Get_t10m_04() {
            if (Layer_t10m_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_04) {
                    t10m_04_value = data_t10m_04.features[0].properties.GRAY_INDEX;
                    Get_t10m_05();
                });
            }
            else {
                Get_t10m_05();
            }
        };
        //---------------------
        function Get_t10m_05() {
            if (Layer_t10m_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_05) {
                    t10m_05_value = data_t10m_05.features[0].properties.GRAY_INDEX;
                    Get_t10m_06();
                });
            }
            else {
                Get_t10m_06();
            }
        };
        //---------------------
        function Get_t10m_06() {
            if (Layer_t10m_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_06) {
                    t10m_06_value = data_t10m_06.features[0].properties.GRAY_INDEX;
                    Get_t10m_07();
                });
            }
            else {
                Get_t10m_07();
            }
        };
        //---------------------
        function Get_t10m_07() {
            if (Layer_t10m_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_07) {
                    t10m_07_value = data_t10m_07.features[0].properties.GRAY_INDEX;
                    Get_t10m_08();
                });
            }
            else {
                Get_t10m_08();
            }
        };
        //---------------------
        function Get_t10m_08() {
            if (Layer_t10m_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_08) {
                    t10m_08_value = data_t10m_08.features[0].properties.GRAY_INDEX;
                    Get_t10m_09();
                });
            }
            else {
                Get_t10m_09();
            }
        };
        //---------------------
        function Get_t10m_09() {
            if (Layer_t10m_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_09) {
                    t10m_09_value = data_t10m_09.features[0].properties.GRAY_INDEX;
                    Get_t10m_10();
                });
            }
            else {
                Get_t10m_10();
            }
        };
        //---------------------
        function Get_t10m_10() {
            if (Layer_t10m_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_10) {
                    t10m_10_value = data_t10m_10.features[0].properties.GRAY_INDEX;
                    Get_t10m_11();
                });
            }
            else {
                Get_t10m_11();
            }
        };
        //---------------------
        function Get_t10m_11() {
            if (Layer_t10m_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_11) {
                    t10m_11_value = data_t10m_11.features[0].properties.GRAY_INDEX;
                    Get_t10m_12();
                });
            }
            else {
                Get_t10m_12();
            }
        };
        //---------------------
        function Get_t10m_12() {
            if (Layer_t10m_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_12) {
                    t10m_12_value = data_t10m_12.features[0].properties.GRAY_INDEX;
                    Get_rainavgesm_year();
                });
            }
            else {
                Get_rainavgesm_year();
            }
        }
        //---------------------
        function Get_rainavgesm_year() {
            if (Layer_rainavgesm_year.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rainavgesm_year + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rainavgesm_year) {
                    rainavgesm_year_value = data_rainavgesm_year.features[0].properties.GRAY_INDEX;
                    Get_rainavgesm_01();
                });
            }
            else {
                Get_rainavgesm_01();
            }
        }
        function Get_rainavgesm_01() {
            if (Layer_rainavgesm_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rainavgesm_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rainavgesm_01) {
                    rainavgesm_01_value = data_rainavgesm_01.features[0].properties.GRAY_INDEX;
                    Get_rainavgesm_02();
                });
            }
            else {
                Get_rainavgesm_02();
            }
        };
        //---------------------
        function Get_rainavgesm_02() {
            if (Layer_rainavgesm_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rainavgesm_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rainavgesm_02) {
                    rainavgesm_02_value = data_rainavgesm_02.features[0].properties.GRAY_INDEX;
                    Get_rainavgesm_03();
                });
            }
            else {
                Get_rainavgesm_03();
            }
        };
        //---------------------
        function Get_rainavgesm_03() {
            if (Layer_rainavgesm_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rainavgesm_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rainavgesm_03) {
                    rainavgesm_03_value = data_rainavgesm_03.features[0].properties.GRAY_INDEX;
                    Get_rainavgesm_04();
                });
            }
            else {
                Get_rainavgesm_04();
            }
        };
        //---------------------
        function Get_rainavgesm_04() {
            if (Layer_rainavgesm_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rainavgesm_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rainavgesm_04) {
                    rainavgesm_04_value = data_rainavgesm_04.features[0].properties.GRAY_INDEX;
                    Get_rainavgesm_05();
                });
            }
            else {
                Get_rainavgesm_05();
            }
        };
        //---------------------
        function Get_rainavgesm_05() {
            if (Layer_rainavgesm_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rainavgesm_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rainavgesm_05) {
                    rainavgesm_05_value = data_rainavgesm_05.features[0].properties.GRAY_INDEX;
                    Get_rainavgesm_06();
                });
            }
            else {
                Get_rainavgesm_06();
            }
        };
        //---------------------
        function Get_rainavgesm_06() {
            if (Layer_rainavgesm_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rainavgesm_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rainavgesm_06) {
                    rainavgesm_06_value = data_rainavgesm_06.features[0].properties.GRAY_INDEX;
                    Get_rainavgesm_07();
                });
            }
            else {
                Get_rainavgesm_07();
            }
        };
        //---------------------
        function Get_rainavgesm_07() {
            if (Layer_rainavgesm_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rainavgesm_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rainavgesm_07) {
                    rainavgesm_07_value = data_rainavgesm_07.features[0].properties.GRAY_INDEX;
                    Get_rainavgesm_08();
                });
            }
            else {
                Get_rainavgesm_08();
            }
        };
        //---------------------
        function Get_rainavgesm_08() {
            if (Layer_rainavgesm_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rainavgesm_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rainavgesm_08) {
                    rainavgesm_08_value = data_rainavgesm_08.features[0].properties.GRAY_INDEX;
                    Get_rainavgesm_09();
                });
            }
            else {
                Get_rainavgesm_09();
            }
        };
        //---------------------
        function Get_rainavgesm_09() {
            if (Layer_rainavgesm_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rainavgesm_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rainavgesm_09) {
                    rainavgesm_09_value = data_rainavgesm_09.features[0].properties.GRAY_INDEX;
                    Get_rainavgesm_10();
                });
            }
            else {
                Get_rainavgesm_10();
            }
        };
        //---------------------
        function Get_rainavgesm_10() {
            if (Layer_rainavgesm_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rainavgesm_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rainavgesm_10) {
                    rainavgesm_10_value = data_rainavgesm_10.features[0].properties.GRAY_INDEX;
                    Get_rainavgesm_11();
                });
            }
            else {
                Get_rainavgesm_11();
            }
        };
        //---------------------
        function Get_rainavgesm_11() {
            if (Layer_rainavgesm_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rainavgesm_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rainavgesm_11) {
                    rainavgesm_11_value = data_rainavgesm_11.features[0].properties.GRAY_INDEX;
                    Get_rainavgesm_12();
                });
            }
            else {
                Get_rainavgesm_12();
            }
        };
        //---------------------
        function Get_rainavgesm_12() {
            if (Layer_rainavgesm_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rainavgesm_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rainavgesm_12) {
                    rainavgesm_12_value = data_rainavgesm_12.features[0].properties.GRAY_INDEX;
                    Get_t10mmax_01();
                });
            }
            else {
                Get_t10mmax_01();
            }
        }
        //---------------------
        function Get_t10mmax_01() {
            if (Layer_t10mmax_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10mmax_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10mmax_01) {
                    t10mmax_01_value = data_t10mmax_01.features[0].properties.GRAY_INDEX;
                    Get_t10mmax_02();
                });
            }
            else {
                Get_t10mmax_02();
            }
        };
        //---------------------
        function Get_t10mmax_02() {
            if (Layer_t10mmax_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10mmax_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10mmax_02) {
                    t10mmax_02_value = data_t10mmax_02.features[0].properties.GRAY_INDEX;
                    Get_t10mmax_03();
                });
            }
            else {
                Get_t10mmax_03();
            }
        };
        //---------------------
        function Get_t10mmax_03() {
            if (Layer_t10mmax_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10mmax_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10mmax_03) {
                    t10mmax_03_value = data_t10mmax_03.features[0].properties.GRAY_INDEX;
                    Get_t10mmax_04();
                });
            }
            else {
                Get_t10mmax_04();
            }
        };
        //---------------------
        function Get_t10mmax_04() {
            if (Layer_t10mmax_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10mmax_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10mmax_04) {
                    t10mmax_04_value = data_t10mmax_04.features[0].properties.GRAY_INDEX;
                    Get_t10mmax_05();
                });
            }
            else {
                Get_t10mmax_05();
            }
        };
        //---------------------
        function Get_t10mmax_05() {
            if (Layer_t10mmax_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10mmax_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10mmax_05) {
                    t10mmax_05_value = data_t10mmax_05.features[0].properties.GRAY_INDEX;
                    Get_t10mmax_06();
                });
            }
            else {
                Get_t10mmax_06();
            }
        };
        //---------------------
        function Get_t10mmax_06() {
            if (Layer_t10mmax_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10mmax_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10mmax_06) {
                    t10mmax_06_value = data_t10mmax_06.features[0].properties.GRAY_INDEX;
                    Get_t10mmax_07();
                });
            }
            else {
                Get_t10mmax_07();
            }
        };
        //---------------------
        function Get_t10mmax_07() {
            if (Layer_t10mmax_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10mmax_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10mmax_07) {
                    t10mmax_07_value = data_t10mmax_07.features[0].properties.GRAY_INDEX;
                    Get_t10mmax_08();
                });
            }
            else {
                Get_t10mmax_08();
            }
        };
        //---------------------
        function Get_t10mmax_08() {
            if (Layer_t10mmax_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10mmax_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10mmax_08) {
                    t10mmax_08_value = data_t10mmax_08.features[0].properties.GRAY_INDEX;
                    Get_t10mmax_09();
                });
            }
            else {
                Get_t10mmax_09();
            }
        };
        //---------------------
        function Get_t10mmax_09() {
            if (Layer_t10mmax_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10mmax_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10mmax_09) {
                    t10mmax_09_value = data_t10mmax_09.features[0].properties.GRAY_INDEX;
                    Get_t10mmax_10();
                });
            }
            else {
                Get_t10mmax_10();
            }
        };
        //---------------------
        function Get_t10mmax_10() {
            if (Layer_t10mmax_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10mmax_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10mmax_10) {
                    t10mmax_10_value = data_t10mmax_10.features[0].properties.GRAY_INDEX;
                    Get_t10mmax_11();
                });
            }
            else {
                Get_t10mmax_11();
            }
        };
        //---------------------
        function Get_t10mmax_11() {
            if (Layer_t10mmax_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10mmax_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10mmax_11) {
                    t10mmax_11_value = data_t10mmax_11.features[0].properties.GRAY_INDEX;
                    Get_t10mmax_12();
                });
            }
            else {
                Get_t10mmax_12();
            }
        };
        //---------------------
        function Get_t10mmax_12() {
            if (Layer_t10mmax_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10mmax_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10mmax_12) {
                    t10mmax_12_value = data_t10mmax_12.features[0].properties.GRAY_INDEX;
                    Get_t10m_min_01();
                });
            }
            else {
                Get_t10m_min_01();
            }
        }
        //---------------------
        function Get_t10m_min_01() {
            if (Layer_t10m_min_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_min_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_min_01) {
                    t10m_min_01_value = data_t10m_min_01.features[0].properties.GRAY_INDEX;
                    Get_t10m_min_02();
                });
            }
            else {
                Get_t10m_min_02();
            }
        };
        //---------------------
        function Get_t10m_min_02() {
            if (Layer_t10m_min_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_min_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_min_02) {
                    t10m_min_02_value = data_t10m_min_02.features[0].properties.GRAY_INDEX;
                    Get_t10m_min_03();
                });
            }
            else {
                Get_t10m_min_03();
            }
        };
        //---------------------
        function Get_t10m_min_03() {
            if (Layer_t10m_min_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_min_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_min_03) {
                    t10m_min_03_value = data_t10m_min_03.features[0].properties.GRAY_INDEX;
                    Get_t10m_min_04();
                });
            }
            else {
                Get_t10m_min_04();
            }
        };
        //---------------------
        function Get_t10m_min_04() {
            if (Layer_t10m_min_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_min_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_min_04) {
                    t10m_min_04_value = data_t10m_min_04.features[0].properties.GRAY_INDEX;
                    Get_t10m_min_05();
                });
            }
            else {
                Get_t10m_min_05();
            }
        };
        //---------------------
        function Get_t10m_min_05() {
            if (Layer_t10m_min_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_min_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_min_05) {
                    t10m_min_05_value = data_t10m_min_05.features[0].properties.GRAY_INDEX;
                    Get_t10m_min_06();
                });
            }
            else {
                Get_t10m_min_06();
            }
        };
        //---------------------
        function Get_t10m_min_06() {
            if (Layer_t10m_min_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_min_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_min_06) {
                    t10m_min_06_value = data_t10m_min_06.features[0].properties.GRAY_INDEX;
                    Get_t10m_min_07();
                });
            }
            else {
                Get_t10m_min_07();
            }
        };
        //---------------------
        function Get_t10m_min_07() {
            if (Layer_t10m_min_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_min_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_min_07) {
                    t10m_min_07_value = data_t10m_min_07.features[0].properties.GRAY_INDEX;
                    Get_t10m_min_08();
                });
            }
            else {
                Get_t10m_min_08();
            }
        };
        //---------------------
        function Get_t10m_min_08() {
            if (Layer_t10m_min_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_min_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_min_08) {
                    t10m_min_08_value = data_t10m_min_08.features[0].properties.GRAY_INDEX;
                    Get_t10m_min_09();
                });
            }
            else {
                Get_t10m_min_09();
            }
        };
        //---------------------
        function Get_t10m_min_09() {
            if (Layer_t10m_min_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_min_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_min_09) {
                    t10m_min_09_value = data_t10m_min_09.features[0].properties.GRAY_INDEX;
                    Get_t10m_min_10();
                });
            }
            else {
                Get_t10m_min_10();
            }
        };
        //---------------------
        function Get_t10m_min_10() {
            if (Layer_t10m_min_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_min_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_min_10) {
                    t10m_min_10_value = data_t10m_min_10.features[0].properties.GRAY_INDEX;
                    Get_t10m_min_11();
                });
            }
            else {
                Get_t10m_min_11();
            }
        };
        //---------------------
        function Get_t10m_min_11() {
            if (Layer_t10m_min_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_min_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_min_11) {
                    t10m_min_11_value = data_t10m_min_11.features[0].properties.GRAY_INDEX;
                    Get_t10m_min_12();
                });
            }
            else {
                Get_t10m_min_12();
            }
        };
        //---------------------
        function Get_t10m_min_12() {
            if (Layer_t10m_min_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_t10m_min_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_t10m_min_12) {
                    t10m_min_12_value = data_t10m_min_12.features[0].properties.GRAY_INDEX;
                    Get_tskinavg_01();
                });
            }
            else {
                Get_tskinavg_01();
            }
        }
        //---------------------
        function Get_tskinavg_01() {
            if (Layer_tskinavg_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_tskinavg_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_tskinavg_01) {
                    tskinavg_01_value = data_tskinavg_01.features[0].properties.GRAY_INDEX;
                    Get_tskinavg_02();
                });
            }
            else {
                Get_tskinavg_02();
            }
        };
        //---------------------
        function Get_tskinavg_02() {
            if (Layer_tskinavg_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_tskinavg_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_tskinavg_02) {
                    tskinavg_02_value = data_tskinavg_02.features[0].properties.GRAY_INDEX;
                    Get_tskinavg_03();
                });
            }
            else {
                Get_tskinavg_03();
            }
        };
        //---------------------
        function Get_tskinavg_03() {
            if (Layer_tskinavg_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_tskinavg_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_tskinavg_03) {
                    tskinavg_03_value = data_tskinavg_03.features[0].properties.GRAY_INDEX;
                    Get_tskinavg_04();
                });
            }
            else {
                Get_tskinavg_04();
            }
        };
        //---------------------
        function Get_tskinavg_04() {
            if (Layer_tskinavg_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_tskinavg_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_tskinavg_04) {
                    tskinavg_04_value = data_tskinavg_04.features[0].properties.GRAY_INDEX;
                    Get_tskinavg_05();
                });
            }
            else {
                Get_tskinavg_05();
            }
        };
        //---------------------
        function Get_tskinavg_05() {
            if (Layer_tskinavg_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_tskinavg_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_tskinavg_05) {
                    tskinavg_05_value = data_tskinavg_05.features[0].properties.GRAY_INDEX;
                    Get_tskinavg_06();
                });
            }
            else {
                Get_tskinavg_06();
            }
        };
        //---------------------
        function Get_tskinavg_06() {
            if (Layer_tskinavg_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_tskinavg_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_tskinavg_06) {
                    tskinavg_06_value = data_tskinavg_06.features[0].properties.GRAY_INDEX;
                    Get_tskinavg_07();
                });
            }
            else {
                Get_tskinavg_07();
            }
        };
        //---------------------
        function Get_tskinavg_07() {
            if (Layer_tskinavg_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_tskinavg_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_tskinavg_07) {
                    tskinavg_07_value = data_tskinavg_07.features[0].properties.GRAY_INDEX;
                    Get_tskinavg_08();
                });
            }
            else {
                Get_tskinavg_08();
            }
        };
        //---------------------
        function Get_tskinavg_08() {
            if (Layer_tskinavg_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_tskinavg_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_tskinavg_08) {
                    tskinavg_08_value = data_tskinavg_08.features[0].properties.GRAY_INDEX;
                    Get_tskinavg_09();
                });
            }
            else {
                Get_tskinavg_09();
            }
        };
        //---------------------
        function Get_tskinavg_09() {
            if (Layer_tskinavg_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_tskinavg_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_tskinavg_09) {
                    tskinavg_09_value = data_tskinavg_09.features[0].properties.GRAY_INDEX;
                    Get_tskinavg_10();
                });
            }
            else {
                Get_tskinavg_10();
            }
        };
        //---------------------
        function Get_tskinavg_10() {
            if (Layer_tskinavg_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_tskinavg_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_tskinavg_10) {
                    tskinavg_10_value = data_tskinavg_10.features[0].properties.GRAY_INDEX;
                    Get_tskinavg_11();
                });
            }
            else {
                Get_tskinavg_11();
            }
        };
        //---------------------
        function Get_tskinavg_11() {
            if (Layer_tskinavg_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_tskinavg_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_tskinavg_11) {
                    tskinavg_11_value = data_tskinavg_11.features[0].properties.GRAY_INDEX;
                    Get_tskinavg_12();
                });
            }
            else {
                Get_tskinavg_12();
            }
        };
        //---------------------
        function Get_tskinavg_12() {
            if (Layer_tskinavg_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_tskinavg_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_tskinavg_12) {
                    tskinavg_12_value = data_tskinavg_12.features[0].properties.GRAY_INDEX;
                    Get_srfalbavg_01();
                });
            }
            else {
                Get_srfalbavg_01();
            }
        }
        //---------------------
        function Get_srfalbavg_01() {
            if (Layer_srfalbavg_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_srfalbavg_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_srfalbavg_01) {
                    srfalbavg_01_value = data_srfalbavg_01.features[0].properties.GRAY_INDEX;
                    Get_srfalbavg_02();
                });
            }
            else {
                Get_srfalbavg_02();
            }
        };
        //---------------------
        function Get_srfalbavg_02() {
            if (Layer_srfalbavg_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_srfalbavg_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_srfalbavg_02) {
                    srfalbavg_02_value = data_srfalbavg_02.features[0].properties.GRAY_INDEX;
                    Get_srfalbavg_03();
                });
            }
            else {
                Get_srfalbavg_03();
            }
        };
        //---------------------
        function Get_srfalbavg_03() {
            if (Layer_srfalbavg_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_srfalbavg_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_srfalbavg_03) {
                    srfalbavg_03_value = data_srfalbavg_03.features[0].properties.GRAY_INDEX;
                    Get_srfalbavg_04();
                });
            }
            else {
                Get_srfalbavg_04();
            }
        };
        //---------------------
        function Get_srfalbavg_04() {
            if (Layer_srfalbavg_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_srfalbavg_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_srfalbavg_04) {
                    srfalbavg_04_value = data_srfalbavg_04.features[0].properties.GRAY_INDEX;
                    Get_srfalbavg_05();
                });
            }
            else {
                Get_srfalbavg_05();
            }
        };
        //---------------------
        function Get_srfalbavg_05() {
            if (Layer_srfalbavg_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_srfalbavg_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_srfalbavg_05) {
                    srfalbavg_05_value = data_srfalbavg_05.features[0].properties.GRAY_INDEX;
                    Get_srfalbavg_06();
                });
            }
            else {
                Get_srfalbavg_06();
            }
        };
        //---------------------
        function Get_srfalbavg_06() {
            if (Layer_srfalbavg_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_srfalbavg_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_srfalbavg_06) {
                    srfalbavg_06_value = data_srfalbavg_06.features[0].properties.GRAY_INDEX;
                    Get_srfalbavg_07();
                });
            }
            else {
                Get_srfalbavg_07();
            }
        };
        //---------------------
        function Get_srfalbavg_07() {
            if (Layer_srfalbavg_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_srfalbavg_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_srfalbavg_07) {
                    srfalbavg_07_value = data_srfalbavg_07.features[0].properties.GRAY_INDEX;
                    Get_srfalbavg_08();
                });
            }
            else {
                Get_srfalbavg_08();
            }
        };
        //---------------------
        function Get_srfalbavg_08() {
            if (Layer_srfalbavg_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_srfalbavg_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_srfalbavg_08) {
                    srfalbavg_08_value = data_srfalbavg_08.features[0].properties.GRAY_INDEX;
                    Get_srfalbavg_09();
                });
            }
            else {
                Get_srfalbavg_09();
            }
        };
        //---------------------
        function Get_srfalbavg_09() {
            if (Layer_srfalbavg_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_srfalbavg_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_srfalbavg_09) {
                    srfalbavg_09_value = data_srfalbavg_09.features[0].properties.GRAY_INDEX;
                    Get_srfalbavg_10();
                });
            }
            else {
                Get_srfalbavg_10();
            }
        };
        //---------------------
        function Get_srfalbavg_10() {
            if (Layer_srfalbavg_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_srfalbavg_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_srfalbavg_10) {
                    srfalbavg_10_value = data_srfalbavg_10.features[0].properties.GRAY_INDEX;
                    Get_srfalbavg_11();
                });
            }
            else {
                Get_srfalbavg_11();
            }
        };
        //---------------------
        function Get_srfalbavg_11() {
            if (Layer_srfalbavg_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_srfalbavg_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_srfalbavg_11) {
                    srfalbavg_11_value = data_srfalbavg_11.features[0].properties.GRAY_INDEX;
                    Get_srfalbavg_12();
                });
            }
            else {
                Get_srfalbavg_12();
            }
        };
        //---------------------
        function Get_srfalbavg_12() {
            if (Layer_srfalbavg_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_srfalbavg_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_srfalbavg_12) {
                    srfalbavg_12_value = data_srfalbavg_12.features[0].properties.GRAY_INDEX;
                    Get_avg_kt_22_01();
                });
            }
            else {
                Get_avg_kt_22_01();
            }
        }
        //---------------------
        function Get_avg_kt_22_01() {
            if (Layer_avg_kt_22_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_kt_22_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_kt_22_01) {
                    avg_kt_22_01_value = data_avg_kt_22_01.features[0].properties.GRAY_INDEX;
                    Get_avg_kt_22_02();
                });
            }
            else {
                Get_avg_kt_22_02();
            }
        };
        //---------------------
        function Get_avg_kt_22_02() {
            if (Layer_avg_kt_22_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_kt_22_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_kt_22_02) {
                    avg_kt_22_02_value = data_avg_kt_22_02.features[0].properties.GRAY_INDEX;
                    Get_avg_kt_22_03();
                });
            }
            else {
                Get_avg_kt_22_03();
            }
        };
        //---------------------
        function Get_avg_kt_22_03() {
            if (Layer_avg_kt_22_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_kt_22_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_kt_22_03) {
                    avg_kt_22_03_value = data_avg_kt_22_03.features[0].properties.GRAY_INDEX;
                    Get_avg_kt_22_04();
                });
            }
            else {
                Get_avg_kt_22_04();
            }
        };
        //---------------------
        function Get_avg_kt_22_04() {
            if (Layer_avg_kt_22_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_kt_22_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_kt_22_04) {
                    avg_kt_22_04_value = data_avg_kt_22_04.features[0].properties.GRAY_INDEX;
                    Get_avg_kt_22_05();
                });
            }
            else {
                Get_avg_kt_22_05();
            }
        };
        //---------------------
        function Get_avg_kt_22_05() {
            if (Layer_avg_kt_22_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_kt_22_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_kt_22_05) {
                    avg_kt_22_05_value = data_avg_kt_22_05.features[0].properties.GRAY_INDEX;
                    Get_avg_kt_22_06();
                });
            }
            else {
                Get_avg_kt_22_06();
            }
        };
        //---------------------
        function Get_avg_kt_22_06() {
            if (Layer_avg_kt_22_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_kt_22_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_kt_22_06) {
                    avg_kt_22_06_value = data_avg_kt_22_06.features[0].properties.GRAY_INDEX;
                    Get_avg_kt_22_07();
                });
            }
            else {
                Get_avg_kt_22_07();
            }
        };
        //---------------------
        function Get_avg_kt_22_07() {
            if (Layer_avg_kt_22_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_kt_22_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_kt_22_07) {
                    avg_kt_22_07_value = data_avg_kt_22_07.features[0].properties.GRAY_INDEX;
                    Get_avg_kt_22_08();
                });
            }
            else {
                Get_avg_kt_22_08();
            }
        };
        //---------------------
        function Get_avg_kt_22_08() {
            if (Layer_avg_kt_22_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_kt_22_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_kt_22_08) {
                    avg_kt_22_08_value = data_avg_kt_22_08.features[0].properties.GRAY_INDEX;
                    Get_avg_kt_22_09();
                });
            }
            else {
                Get_avg_kt_22_09();
            }
        };
        //---------------------
        function Get_avg_kt_22_09() {
            if (Layer_avg_kt_22_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_kt_22_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_kt_22_09) {
                    avg_kt_22_09_value = data_avg_kt_22_09.features[0].properties.GRAY_INDEX;
                    Get_avg_kt_22_10();
                });
            }
            else {
                Get_avg_kt_22_10();
            }
        };
        //---------------------
        function Get_avg_kt_22_10() {
            if (Layer_avg_kt_22_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_kt_22_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_kt_22_10) {
                    avg_kt_22_10_value = data_avg_kt_22_10.features[0].properties.GRAY_INDEX;
                    Get_avg_kt_22_11();
                });
            }
            else {
                Get_avg_kt_22_11();
            }
        };
        //---------------------
        function Get_avg_kt_22_11() {
            if (Layer_avg_kt_22_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_kt_22_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_kt_22_11) {
                    avg_kt_22_11_value = data_avg_kt_22_11.features[0].properties.GRAY_INDEX;
                    Get_avg_kt_22_12();
                });
            }
            else {
                Get_avg_kt_22_12();
            }
        };
        //---------------------
        function Get_avg_kt_22_12() {
            if (Layer_avg_kt_22_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_kt_22_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_kt_22_12) {
                    avg_kt_22_12_value = data_avg_kt_22_12.features[0].properties.GRAY_INDEX;
                    Get_avg_nkt_22_01();
                });
            }
            else {
                Get_avg_nkt_22_01();
            }
        }
        //---------------------
        function Get_avg_nkt_22_01() {
            if (Layer_avg_nkt_22_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_nkt_22_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_nkt_22_01) {
                    avg_nkt_22_01_value = data_avg_nkt_22_01.features[0].properties.GRAY_INDEX;
                    Get_avg_nkt_22_02();
                });
            }
            else {
                Get_avg_nkt_22_02();
            }
        };
        //---------------------
        function Get_avg_nkt_22_02() {
            if (Layer_avg_nkt_22_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_nkt_22_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_nkt_22_02) {
                    avg_nkt_22_02_value = data_avg_nkt_22_02.features[0].properties.GRAY_INDEX;
                    Get_avg_nkt_22_03();
                });
            }
            else {
                Get_avg_nkt_22_03();
            }
        };
        //---------------------
        function Get_avg_nkt_22_03() {
            if (Layer_avg_nkt_22_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_nkt_22_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_nkt_22_03) {
                    avg_nkt_22_03_value = data_avg_nkt_22_03.features[0].properties.GRAY_INDEX;
                    Get_avg_nkt_22_04();
                });
            }
            else {
                Get_avg_nkt_22_04();
            }
        };
        //---------------------
        function Get_avg_nkt_22_04() {
            if (Layer_avg_nkt_22_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_nkt_22_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_nkt_22_04) {
                    avg_nkt_22_04_value = data_avg_nkt_22_04.features[0].properties.GRAY_INDEX;
                    Get_avg_nkt_22_05();
                });
            }
            else {
                Get_avg_nkt_22_05();
            }
        };
        //---------------------
        function Get_avg_nkt_22_05() {
            if (Layer_avg_nkt_22_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_nkt_22_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_nkt_22_05) {
                    avg_nkt_22_05_value = data_avg_nkt_22_05.features[0].properties.GRAY_INDEX;
                    Get_avg_nkt_22_06();
                });
            }
            else {
                Get_avg_nkt_22_06();
            }
        };
        //---------------------
        function Get_avg_nkt_22_06() {
            if (Layer_avg_nkt_22_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_nkt_22_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_nkt_22_06) {
                    avg_nkt_22_06_value = data_avg_nkt_22_06.features[0].properties.GRAY_INDEX;
                    Get_avg_nkt_22_07();
                });
            }
            else {
                Get_avg_nkt_22_07();
            }
        };
        //---------------------
        function Get_avg_nkt_22_07() {
            if (Layer_avg_nkt_22_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_nkt_22_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_nkt_22_07) {
                    avg_nkt_22_07_value = data_avg_nkt_22_07.features[0].properties.GRAY_INDEX;
                    Get_avg_nkt_22_08();
                });
            }
            else {
                Get_avg_nkt_22_08();
            }
        };
        //---------------------
        function Get_avg_nkt_22_08() {
            if (Layer_avg_nkt_22_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_nkt_22_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_nkt_22_08) {
                    avg_nkt_22_08_value = data_avg_nkt_22_08.features[0].properties.GRAY_INDEX;
                    Get_avg_nkt_22_09();
                });
            }
            else {
                Get_avg_nkt_22_09();
            }
        };
        //---------------------
        function Get_avg_nkt_22_09() {
            if (Layer_avg_nkt_22_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_nkt_22_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_nkt_22_09) {
                    avg_nkt_22_09_value = data_avg_nkt_22_09.features[0].properties.GRAY_INDEX;
                    Get_avg_nkt_22_10();
                });
            }
            else {
                Get_avg_nkt_22_10();
            }
        };
        //---------------------
        function Get_avg_nkt_22_10() {
            if (Layer_avg_nkt_22_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_nkt_22_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_nkt_22_10) {
                    avg_nkt_22_10_value = data_avg_nkt_22_10.features[0].properties.GRAY_INDEX;
                    Get_avg_nkt_22_11();
                });
            }
            else {
                Get_avg_nkt_22_11();
            }
        };
        //---------------------
        function Get_avg_nkt_22_11() {
            if (Layer_avg_nkt_22_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_nkt_22_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_nkt_22_11) {
                    avg_nkt_22_11_value = data_avg_nkt_22_11.features[0].properties.GRAY_INDEX;
                    Get_avg_nkt_22_12();
                });
            }
            else {
                Get_avg_nkt_22_12();
            }
        };
        //---------------------
        function Get_avg_nkt_22_12() {
            if (Layer_avg_nkt_22_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_avg_nkt_22_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_avg_nkt_22_12) {
                    avg_nkt_22_12_value = data_avg_nkt_22_12.features[0].properties.GRAY_INDEX;
                    Get_day_cld_22_01();
                });
            }
            else {
                Get_day_cld_22_01();
            }
        }
        //---------------------
        function Get_day_cld_22_01() {
            if (Layer_day_cld_22_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_day_cld_22_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_day_cld_22_01) {
                    day_cld_22_01_value = data_day_cld_22_01.features[0].properties.GRAY_INDEX;
                    Get_day_cld_22_02();
                });
            }
            else {
                Get_day_cld_22_02();
            }
        };
        //---------------------
        function Get_day_cld_22_02() {
            if (Layer_day_cld_22_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_day_cld_22_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_day_cld_22_02) {
                    day_cld_22_02_value = data_day_cld_22_02.features[0].properties.GRAY_INDEX;
                    Get_day_cld_22_03();
                });
            }
            else {
                Get_day_cld_22_03();
            }
        };
        //---------------------
        function Get_day_cld_22_03() {
            if (Layer_day_cld_22_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_day_cld_22_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_day_cld_22_03) {
                    day_cld_22_03_value = data_day_cld_22_03.features[0].properties.GRAY_INDEX;
                    Get_day_cld_22_04();
                });
            }
            else {
                Get_day_cld_22_04();
            }
        };
        //---------------------
        function Get_day_cld_22_04() {
            if (Layer_day_cld_22_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_day_cld_22_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_day_cld_22_04) {
                    day_cld_22_04_value = data_day_cld_22_04.features[0].properties.GRAY_INDEX;
                    Get_day_cld_22_05();
                });
            }
            else {
                Get_day_cld_22_05();
            }
        };
        //---------------------
        function Get_day_cld_22_05() {
            if (Layer_day_cld_22_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_day_cld_22_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_day_cld_22_05) {
                    day_cld_22_05_value = data_day_cld_22_05.features[0].properties.GRAY_INDEX;
                    Get_day_cld_22_06();
                });
            }
            else {
                Get_day_cld_22_06();
            }
        };
        //---------------------
        function Get_day_cld_22_06() {
            if (Layer_day_cld_22_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_day_cld_22_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_day_cld_22_06) {
                    day_cld_22_06_value = data_day_cld_22_06.features[0].properties.GRAY_INDEX;
                    Get_day_cld_22_07();
                });
            }
            else {
                Get_day_cld_22_07();
            }
        };
        //---------------------
        function Get_day_cld_22_07() {
            if (Layer_day_cld_22_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_day_cld_22_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_day_cld_22_07) {
                    day_cld_22_07_value = data_day_cld_22_07.features[0].properties.GRAY_INDEX;
                    Get_day_cld_22_08();
                });
            }
            else {
                Get_day_cld_22_08();
            }
        };
        //---------------------
        function Get_day_cld_22_08() {
            if (Layer_day_cld_22_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_day_cld_22_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_day_cld_22_08) {
                    day_cld_22_08_value = data_day_cld_22_08.features[0].properties.GRAY_INDEX;
                    Get_day_cld_22_09();
                });
            }
            else {
                Get_day_cld_22_09();
            }
        };
        //---------------------
        function Get_day_cld_22_09() {
            if (Layer_day_cld_22_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_day_cld_22_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_day_cld_22_09) {
                    day_cld_22_09_value = data_day_cld_22_09.features[0].properties.GRAY_INDEX;
                    Get_day_cld_22_10();
                });
            }
            else {
                Get_day_cld_22_10();
            }
        };
        //---------------------
        function Get_day_cld_22_10() {
            if (Layer_day_cld_22_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_day_cld_22_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_day_cld_22_10) {
                    day_cld_22_10_value = data_day_cld_22_10.features[0].properties.GRAY_INDEX;
                    Get_day_cld_22_11();
                });
            }
            else {
                Get_day_cld_22_11();
            }
        };
        //---------------------
        function Get_day_cld_22_11() {
            if (Layer_day_cld_22_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_day_cld_22_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_day_cld_22_11) {
                    day_cld_22_11_value = data_day_cld_22_11.features[0].properties.GRAY_INDEX;
                    Get_day_cld_22_12();
                });
            }
            else {
                Get_day_cld_22_12();
            }
        };
        //---------------------
        function Get_day_cld_22_12() {
            if (Layer_day_cld_22_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_day_cld_22_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_day_cld_22_12) {
                    day_cld_22_12_value = data_day_cld_22_12.features[0].properties.GRAY_INDEX;
                    Get_daylghtav_01();
                });
            }
            else {
                Get_daylghtav_01();
            }
        }
        //---------------------
        function Get_daylghtav_01() {
            if (Layer_daylghtav_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_daylghtav_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_daylghtav_01) {
                    daylghtav_01_value = data_daylghtav_01.features[0].properties.GRAY_INDEX;
                    Get_daylghtav_02();
                });
            }
            else {
                Get_daylghtav_02();
            }
        };
        //---------------------
        function Get_daylghtav_02() {
            if (Layer_daylghtav_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_daylghtav_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_daylghtav_02) {
                    daylghtav_02_value = data_daylghtav_02.features[0].properties.GRAY_INDEX;
                    Get_daylghtav_03();
                });
            }
            else {
                Get_daylghtav_03();
            }
        };
        //---------------------
        function Get_daylghtav_03() {
            if (Layer_daylghtav_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_daylghtav_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_daylghtav_03) {
                    daylghtav_03_value = data_daylghtav_03.features[0].properties.GRAY_INDEX;
                    Get_daylghtav_04();
                });
            }
            else {
                Get_daylghtav_04();
            }
        };
        //---------------------
        function Get_daylghtav_04() {
            if (Layer_daylghtav_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_daylghtav_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_daylghtav_04) {
                    daylghtav_04_value = data_daylghtav_04.features[0].properties.GRAY_INDEX;
                    Get_daylghtav_05();
                });
            }
            else {
                Get_daylghtav_05();
            }
        };
        //---------------------
        function Get_daylghtav_05() {
            if (Layer_daylghtav_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_daylghtav_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_daylghtav_05) {
                    daylghtav_05_value = data_daylghtav_05.features[0].properties.GRAY_INDEX;
                    Get_daylghtav_06();
                });
            }
            else {
                Get_daylghtav_06();
            }
        };
        //---------------------
        function Get_daylghtav_06() {
            if (Layer_daylghtav_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_daylghtav_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_daylghtav_06) {
                    daylghtav_06_value = data_daylghtav_06.features[0].properties.GRAY_INDEX;
                    Get_daylghtav_07();
                });
            }
            else {
                Get_daylghtav_07();
            }
        };
        //---------------------
        function Get_daylghtav_07() {
            if (Layer_daylghtav_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_daylghtav_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_daylghtav_07) {
                    daylghtav_07_value = data_daylghtav_07.features[0].properties.GRAY_INDEX;
                    Get_daylghtav_08();
                });
            }
            else {
                Get_daylghtav_08();
            }
        };
        //---------------------
        function Get_daylghtav_08() {
            if (Layer_daylghtav_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_daylghtav_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_daylghtav_08) {
                    daylghtav_08_value = data_daylghtav_08.features[0].properties.GRAY_INDEX;
                    Get_daylghtav_09();
                });
            }
            else {
                Get_daylghtav_09();
            }
        };
        //---------------------
        function Get_daylghtav_09() {
            if (Layer_daylghtav_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_daylghtav_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_daylghtav_09) {
                    daylghtav_09_value = data_daylghtav_09.features[0].properties.GRAY_INDEX;
                    Get_daylghtav_10();
                });
            }
            else {
                Get_daylghtav_10();
            }
        };
        //---------------------
        function Get_daylghtav_10() {
            if (Layer_daylghtav_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_daylghtav_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_daylghtav_10) {
                    daylghtav_10_value = data_daylghtav_10.features[0].properties.GRAY_INDEX;
                    Get_daylghtav_11();
                });
            }
            else {
                Get_daylghtav_11();
            }
        };
        //---------------------
        function Get_daylghtav_11() {
            if (Layer_daylghtav_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_daylghtav_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_daylghtav_11) {
                    daylghtav_11_value = data_daylghtav_11.features[0].properties.GRAY_INDEX;
                    Get_daylghtav_12();
                });
            }
            else {
                Get_daylghtav_12();
            }
        };
        //---------------------
        function Get_daylghtav_12() {
            if (Layer_daylghtav_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_daylghtav_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_daylghtav_12) {
                    daylghtav_12_value = data_daylghtav_12.features[0].properties.GRAY_INDEX;
                    Get_p_clr_cky_year();
                });
            }
            else {
                Get_p_clr_cky_year();
            }
        }
        //---------------------
        function Get_p_clr_cky_year() {
            if (Layer_p_clr_cky_year.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_clr_cky_year + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_clr_cky_year) {
                    p_clr_cky_year_value = data_p_clr_cky_year.features[0].properties.GRAY_INDEX;
                    Get_p_clr_cky_01();
                });
            }
            else {
                Get_p_clr_cky_01();
            }
        };
        //---------------------
        function Get_p_clr_cky_01() {
            if (Layer_p_clr_cky_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_clr_cky_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_clr_cky_01) {
                    p_clr_cky_01_value = data_p_clr_cky_01.features[0].properties.GRAY_INDEX;
                    Get_p_clr_cky_02();
                });
            }
            else {
                Get_p_clr_cky_02();
            }
        };
        //---------------------
        function Get_p_clr_cky_02() {
            if (Layer_p_clr_cky_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_clr_cky_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_clr_cky_02) {
                    p_clr_cky_02_value = data_p_clr_cky_02.features[0].properties.GRAY_INDEX;
                    Get_p_clr_cky_03();
                });
            }
            else {
                Get_p_clr_cky_03();
            }
        };
        //---------------------
        function Get_p_clr_cky_03() {
            if (Layer_p_clr_cky_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_clr_cky_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_clr_cky_03) {
                    p_clr_cky_03_value = data_p_clr_cky_03.features[0].properties.GRAY_INDEX;
                    Get_p_clr_cky_04();
                });
            }
            else {
                Get_p_clr_cky_04();
            }
        };
        //---------------------
        function Get_p_clr_cky_04() {
            if (Layer_p_clr_cky_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_clr_cky_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_clr_cky_04) {
                    p_clr_cky_04_value = data_p_clr_cky_04.features[0].properties.GRAY_INDEX;
                    Get_p_clr_cky_05();
                });
            }
            else {
                Get_p_clr_cky_05();
            }
        };
        //---------------------
        function Get_p_clr_cky_05() {
            if (Layer_p_clr_cky_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_clr_cky_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_clr_cky_05) {
                    p_clr_cky_05_value = data_p_clr_cky_05.features[0].properties.GRAY_INDEX;
                    Get_p_clr_cky_06();
                });
            }
            else {
                Get_p_clr_cky_06();
            }
        };
        //---------------------
        function Get_p_clr_cky_06() {
            if (Layer_p_clr_cky_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_clr_cky_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_clr_cky_06) {
                    p_clr_cky_06_value = data_p_clr_cky_06.features[0].properties.GRAY_INDEX;
                    Get_p_clr_cky_07();
                });
            }
            else {
                Get_p_clr_cky_07();
            }
        };
        //---------------------
        function Get_p_clr_cky_07() {
            if (Layer_p_clr_cky_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_clr_cky_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_clr_cky_07) {
                    p_clr_cky_07_value = data_p_clr_cky_07.features[0].properties.GRAY_INDEX;
                    Get_p_clr_cky_08();
                });
            }
            else {
                Get_p_clr_cky_08();
            }
        };
        //---------------------
        function Get_p_clr_cky_08() {
            if (Layer_p_clr_cky_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_clr_cky_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_clr_cky_08) {
                    p_clr_cky_08_value = data_p_clr_cky_08.features[0].properties.GRAY_INDEX;
                    Get_p_clr_cky_09();
                });
            }
            else {
                Get_p_clr_cky_09();
            }
        };
        //---------------------
        function Get_p_clr_cky_09() {
            if (Layer_p_clr_cky_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_clr_cky_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_clr_cky_09) {
                    p_clr_cky_09_value = data_p_clr_cky_09.features[0].properties.GRAY_INDEX;
                    Get_p_clr_cky_10();
                });
            }
            else {
                Get_p_clr_cky_10();
            }
        };
        //---------------------
        function Get_p_clr_cky_10() {
            if (Layer_p_clr_cky_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_clr_cky_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_clr_cky_10) {
                    p_clr_cky_10_value = data_p_clr_cky_10.features[0].properties.GRAY_INDEX;
                    Get_p_clr_cky_11();
                });
            }
            else {
                Get_p_clr_cky_11();
            }
        };
        //---------------------
        function Get_p_clr_cky_11() {
            if (Layer_p_clr_cky_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_clr_cky_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_clr_cky_11) {
                    p_clr_cky_11_value = data_p_clr_cky_11.features[0].properties.GRAY_INDEX;
                    Get_p_clr_cky_12();
                });
            }
            else {
                Get_p_clr_cky_12();
            }
        };
        //---------------------
        function Get_p_clr_cky_12() {
            if (Layer_p_clr_cky_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_clr_cky_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_clr_cky_12) {
                    p_clr_cky_12_value = data_p_clr_cky_12.features[0].properties.GRAY_INDEX;
                    Get_p_swv_dwn_year();
                });
            }
            else {
                Get_p_swv_dwn_year();
            }
        };
        //---------------------
        function Get_p_swv_dwn_year() {
            if (Layer_p_swv_dwn_year.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_swv_dwn_year + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_swv_dwn_year) {
                    p_swv_dwn_year_value = data_p_swv_dwn_year.features[0].properties.GRAY_INDEX;
                    Get_p_swv_dwn_01();
                });
            }
            else {
                Get_p_swv_dwn_01();
            }
        };
        //---------------------
        function Get_p_swv_dwn_01() {
            if (Layer_p_swv_dwn_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_swv_dwn_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_swv_dwn_01) {
                    p_swv_dwn_01_value = data_p_swv_dwn_01.features[0].properties.GRAY_INDEX;
                    Get_p_swv_dwn_02();
                });
            }
            else {
                Get_p_swv_dwn_02();
            }
        };
        //---------------------
        function Get_p_swv_dwn_02() {
            if (Layer_p_swv_dwn_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_swv_dwn_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_swv_dwn_02) {
                    p_swv_dwn_02_value = data_p_swv_dwn_02.features[0].properties.GRAY_INDEX;
                    Get_p_swv_dwn_03();
                });
            }
            else {
                Get_p_swv_dwn_03();
            }
        };
        //---------------------
        function Get_p_swv_dwn_03() {
            if (Layer_p_swv_dwn_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_swv_dwn_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_swv_dwn_03) {
                    p_swv_dwn_03_value = data_p_swv_dwn_03.features[0].properties.GRAY_INDEX;
                    Get_p_swv_dwn_04();
                });
            }
            else {
                Get_p_swv_dwn_04();
            }
        };
        //---------------------
        function Get_p_swv_dwn_04() {
            if (Layer_p_swv_dwn_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_swv_dwn_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_swv_dwn_04) {
                    p_swv_dwn_04_value = data_p_swv_dwn_04.features[0].properties.GRAY_INDEX;
                    Get_p_swv_dwn_05();
                });
            }
            else {
                Get_p_swv_dwn_05();
            }
        };
        //---------------------
        function Get_p_swv_dwn_05() {
            if (Layer_p_swv_dwn_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_swv_dwn_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_swv_dwn_05) {
                    p_swv_dwn_05_value = data_p_swv_dwn_05.features[0].properties.GRAY_INDEX;
                    Get_p_swv_dwn_06();
                });
            }
            else {
                Get_p_swv_dwn_06();
            }
        };
        //---------------------
        function Get_p_swv_dwn_06() {
            if (Layer_p_swv_dwn_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_swv_dwn_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_swv_dwn_06) {
                    p_swv_dwn_06_value = data_p_swv_dwn_06.features[0].properties.GRAY_INDEX;
                    Get_p_swv_dwn_07();
                });
            }
            else {
                Get_p_swv_dwn_07();
            }
        };
        //---------------------
        function Get_p_swv_dwn_07() {
            if (Layer_p_swv_dwn_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_swv_dwn_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_swv_dwn_07) {
                    p_swv_dwn_07_value = data_p_swv_dwn_07.features[0].properties.GRAY_INDEX;
                    Get_p_swv_dwn_08();
                });
            }
            else {
                Get_p_swv_dwn_08();
            }
        };
        //---------------------
        function Get_p_swv_dwn_08() {
            if (Layer_p_swv_dwn_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_swv_dwn_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_swv_dwn_08) {
                    p_swv_dwn_08_value = data_p_swv_dwn_08.features[0].properties.GRAY_INDEX;
                    Get_p_swv_dwn_09();
                });
            }
            else {
                Get_p_swv_dwn_09();
            }
        };
        //---------------------
        function Get_p_swv_dwn_09() {
            if (Layer_p_swv_dwn_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_swv_dwn_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_swv_dwn_09) {
                    p_swv_dwn_09_value = data_p_swv_dwn_09.features[0].properties.GRAY_INDEX;
                    Get_p_swv_dwn_10();
                });
            }
            else {
                Get_p_swv_dwn_10();
            }
        };
        //---------------------
        function Get_p_swv_dwn_10() {
            if (Layer_p_swv_dwn_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_swv_dwn_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_swv_dwn_10) {
                    p_swv_dwn_10_value = data_p_swv_dwn_10.features[0].properties.GRAY_INDEX;
                    Get_p_swv_dwn_11();
                });
            }
            else {
                Get_p_swv_dwn_11();
            }
        };
        //---------------------
        function Get_p_swv_dwn_11() {
            if (Layer_p_swv_dwn_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_swv_dwn_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_swv_dwn_11) {
                    p_swv_dwn_11_value = data_p_swv_dwn_11.features[0].properties.GRAY_INDEX;
                    Get_p_swv_dwn_12();
                });
            }
            else {
                Get_p_swv_dwn_12();
            }
        };
        //---------------------
        function Get_p_swv_dwn_12() {
            if (Layer_p_swv_dwn_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_swv_dwn_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_swv_dwn_12) {
                    p_swv_dwn_12_value = data_p_swv_dwn_12.features[0].properties.GRAY_INDEX;
                    Get_p_toa_dwn_year();
                });
            }
            else {
                Get_p_toa_dwn_year();
            }
        };
        //---------------------
        function Get_p_toa_dwn_year() {
            if (Layer_p_toa_dwn_year.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_toa_dwn_year + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_toa_dwn_year) {
                    p_toa_dwn_year_value = data_p_toa_dwn_year.features[0].properties.GRAY_INDEX;
                    Get_p_toa_dwn_01();
                });
            }
            else {
                Get_p_toa_dwn_01();
            }
        };
        //---------------------
        function Get_p_toa_dwn_01() {
            if (Layer_p_toa_dwn_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_toa_dwn_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_toa_dwn_01) {
                    p_toa_dwn_01_value = data_p_toa_dwn_01.features[0].properties.GRAY_INDEX;
                    Get_p_toa_dwn_02();
                });
            }
            else {
                Get_p_toa_dwn_02();
            }
        };
        //---------------------
        function Get_p_toa_dwn_02() {
            if (Layer_p_toa_dwn_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_toa_dwn_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_toa_dwn_02) {
                    p_toa_dwn_02_value = data_p_toa_dwn_02.features[0].properties.GRAY_INDEX;
                    Get_p_toa_dwn_03();
                });
            }
            else {
                Get_p_toa_dwn_03();
            }
        };
        //---------------------
        function Get_p_toa_dwn_03() {
            if (Layer_p_toa_dwn_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_toa_dwn_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_toa_dwn_03) {
                    p_toa_dwn_03_value = data_p_toa_dwn_03.features[0].properties.GRAY_INDEX;
                    Get_p_toa_dwn_04();
                });
            }
            else {
                Get_p_toa_dwn_04();
            }
        };
        //---------------------
        function Get_p_toa_dwn_04() {
            if (Layer_p_toa_dwn_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_toa_dwn_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_toa_dwn_04) {
                    p_toa_dwn_04_value = data_p_toa_dwn_04.features[0].properties.GRAY_INDEX;
                    Get_p_toa_dwn_05();
                });
            }
            else {
                Get_p_toa_dwn_05();
            }
        };
        //---------------------
        function Get_p_toa_dwn_05() {
            if (Layer_p_toa_dwn_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_toa_dwn_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_toa_dwn_05) {
                    p_toa_dwn_05_value = data_p_toa_dwn_05.features[0].properties.GRAY_INDEX;
                    Get_p_toa_dwn_06();
                });
            }
            else {
                Get_p_toa_dwn_06();
            }
        };
        //---------------------
        function Get_p_toa_dwn_06() {
            if (Layer_p_toa_dwn_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_toa_dwn_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_toa_dwn_06) {
                    p_toa_dwn_06_value = data_p_toa_dwn_06.features[0].properties.GRAY_INDEX;
                    Get_p_toa_dwn_07();
                });
            }
            else {
                Get_p_toa_dwn_07();
            }
        };
        //---------------------
        function Get_p_toa_dwn_07() {
            if (Layer_p_toa_dwn_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_toa_dwn_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_toa_dwn_07) {
                    p_toa_dwn_07_value = data_p_toa_dwn_07.features[0].properties.GRAY_INDEX;
                    Get_p_toa_dwn_08();
                });
            }
            else {
                Get_p_toa_dwn_08();
            }
        };
        //---------------------
        function Get_p_toa_dwn_08() {
            if (Layer_p_toa_dwn_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_toa_dwn_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_toa_dwn_08) {
                    p_toa_dwn_08_value = data_p_toa_dwn_08.features[0].properties.GRAY_INDEX;
                    Get_p_toa_dwn_09();
                });
            }
            else {
                Get_p_toa_dwn_09();
            }
        };
        //---------------------
        function Get_p_toa_dwn_09() {
            if (Layer_p_toa_dwn_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_toa_dwn_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_toa_dwn_09) {
                    p_toa_dwn_09_value = data_p_toa_dwn_09.features[0].properties.GRAY_INDEX;
                    Get_p_toa_dwn_10();
                });
            }
            else {
                Get_p_toa_dwn_10();
            }
        };
        //---------------------
        function Get_p_toa_dwn_10() {
            if (Layer_p_toa_dwn_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_toa_dwn_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_toa_dwn_10) {
                    p_toa_dwn_10_value = data_p_toa_dwn_10.features[0].properties.GRAY_INDEX;
                    Get_p_toa_dwn_11();
                });
            }
            else {
                Get_p_toa_dwn_11();
            }
        };
        //---------------------
        function Get_p_toa_dwn_11() {
            if (Layer_p_toa_dwn_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_toa_dwn_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_toa_dwn_11) {
                    p_toa_dwn_11_value = data_p_toa_dwn_11.features[0].properties.GRAY_INDEX;
                    Get_p_toa_dwn_12();
                });
            }
            else {
                Get_p_toa_dwn_12();
            }
        };
        //---------------------
        function Get_p_toa_dwn_12() {
            if (Layer_p_toa_dwn_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_p_toa_dwn_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_p_toa_dwn_12) {
                    p_toa_dwn_12_value = data_p_toa_dwn_12.features[0].properties.GRAY_INDEX;
                    Get_dni_year();
                });
            }
            else {
                Get_dni_year();
            }
        };
        //---------------------
        function Get_dni_year() {
            if (Layer_dni_year.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_dni_year + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_dni_year) {
                    dni_year_value = data_dni_year.features[0].properties.GRAY_INDEX;
                    Get_dni_01();
                });
            }
            else {
                Get_dni_01();
            }
        };
        //---------------------
        function Get_dni_01() {
            if (Layer_dni_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_dni_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_dni_01) {
                    dni_01_value = data_dni_01.features[0].properties.GRAY_INDEX;
                    Get_dni_02();
                });
            }
            else {
                Get_dni_02();
            }
        };
        //---------------------
        function Get_dni_02() {
            if (Layer_dni_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_dni_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_dni_02) {
                    dni_02_value = data_dni_02.features[0].properties.GRAY_INDEX;
                    Get_dni_03();
                });
            }
            else {
                Get_dni_03();
            }
        };
        //---------------------
        function Get_dni_03() {
            if (Layer_dni_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_dni_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_dni_03) {
                    dni_03_value = data_dni_03.features[0].properties.GRAY_INDEX;
                    Get_dni_04();
                });
            }
            else {
                Get_dni_04();
            }
        };
        //---------------------
        function Get_dni_04() {
            if (Layer_dni_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_dni_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_dni_04) {
                    dni_04_value = data_dni_04.features[0].properties.GRAY_INDEX;
                    Get_dni_05();
                });
            }
            else {
                Get_dni_05();
            }
        };
        //---------------------
        function Get_dni_05() {
            if (Layer_dni_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_dni_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_dni_05) {
                    dni_05_value = data_dni_05.features[0].properties.GRAY_INDEX;
                    Get_dni_06();
                });
            }
            else {
                Get_dni_06();
            }
        };
        //---------------------
        function Get_dni_06() {
            if (Layer_dni_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_dni_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_dni_06) {
                    dni_06_value = data_dni_06.features[0].properties.GRAY_INDEX;
                    Get_dni_07();
                });
            }
            else {
                Get_dni_07();
            }
        };
        //---------------------
        function Get_dni_07() {
            if (Layer_dni_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_dni_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_dni_07) {
                    dni_07_value = data_dni_07.features[0].properties.GRAY_INDEX;
                    Get_dni_08();
                });
            }
            else {
                Get_dni_08();
            }
        };
        //---------------------
        function Get_dni_08() {
            if (Layer_dni_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_dni_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_dni_08) {
                    dni_08_value = data_dni_08.features[0].properties.GRAY_INDEX;
                    Get_dni_09();
                });
            }
            else {
                Get_dni_09();
            }
        };
        //---------------------
        function Get_dni_09() {
            if (Layer_dni_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_dni_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_dni_09) {
                    dni_09_value = data_dni_09.features[0].properties.GRAY_INDEX;
                    Get_dni_10();
                });
            }
            else {
                Get_dni_10();
            }
        };
        //---------------------
        function Get_dni_10() {
            if (Layer_dni_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_dni_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_dni_10) {
                    dni_10_value = data_dni_10.features[0].properties.GRAY_INDEX;
                    Get_dni_11();
                });
            }
            else {
                Get_dni_11();
            }
        };
        //---------------------
        function Get_dni_11() {
            if (Layer_dni_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_dni_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_dni_11) {
                    dni_11_value = data_dni_11.features[0].properties.GRAY_INDEX;
                    Get_dni_12();
                });
            }
            else {
                Get_dni_12();
            }
        };
        //---------------------
        function Get_dni_12() {
            if (Layer_dni_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_dni_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_dni_12) {
                    dni_12_value = data_dni_12.features[0].properties.GRAY_INDEX;
                    Get_sis_klr_year();
                });
            }
            else {
                Get_sis_klr_year();
            }
        };
        //---------------------
        function Get_sis_klr_year() {
            if (Layer_sis_klr_year.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_klr_year + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_klr_year) {
                    sis_klr_year_value = data_sis_klr_year.features[0].properties.GRAY_INDEX;
                    Get_sis_klr_01();
                });
            }
            else {
                Get_sis_klr_01();
            }
        };
        //---------------------
        function Get_sis_klr_01() {
            if (Layer_sis_klr_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_klr_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_klr_01) {
                    sis_klr_01_value = data_sis_klr_01.features[0].properties.GRAY_INDEX;
                    Get_sis_klr_02();
                });
            }
            else {
                Get_sis_klr_02();
            }
        };
        //---------------------
        function Get_sis_klr_02() {
            if (Layer_sis_klr_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_klr_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_klr_02) {
                    sis_klr_02_value = data_sis_klr_02.features[0].properties.GRAY_INDEX;
                    Get_sis_klr_03();
                });
            }
            else {
                Get_sis_klr_03();
            }
        };
        //---------------------
        function Get_sis_klr_03() {
            if (Layer_sis_klr_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_klr_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_klr_03) {
                    sis_klr_03_value = data_sis_klr_03.features[0].properties.GRAY_INDEX;
                    Get_sis_klr_04();
                });
            }
            else {
                Get_sis_klr_04();
            }
        };
        //---------------------
        function Get_sis_klr_04() {
            if (Layer_sis_klr_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_klr_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_klr_04) {
                    sis_klr_04_value = data_sis_klr_04.features[0].properties.GRAY_INDEX;
                    Get_sis_klr_05();
                });
            }
            else {
                Get_sis_klr_05();
            }
        };
        //---------------------
        function Get_sis_klr_05() {
            if (Layer_sis_klr_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_klr_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_klr_05) {
                    sis_klr_05_value = data_sis_klr_05.features[0].properties.GRAY_INDEX;
                    Get_sis_klr_06();
                });
            }
            else {
                Get_sis_klr_06();
            }
        };
        //---------------------
        function Get_sis_klr_06() {
            if (Layer_sis_klr_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_klr_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_klr_06) {
                    sis_klr_06_value = data_sis_klr_06.features[0].properties.GRAY_INDEX;
                    Get_sis_klr_07();
                });
            }
            else {
                Get_sis_klr_07();
            }
        };
        //---------------------
        function Get_sis_klr_07() {
            if (Layer_sis_klr_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_klr_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_klr_07) {
                    sis_klr_07_value = data_sis_klr_07.features[0].properties.GRAY_INDEX;
                    Get_sis_klr_08();
                });
            }
            else {
                Get_sis_klr_08();
            }
        };
        //---------------------
        function Get_sis_klr_08() {
            if (Layer_sis_klr_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_klr_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_klr_08) {
                    sis_klr_08_value = data_sis_klr_08.features[0].properties.GRAY_INDEX;
                    Get_sis_klr_09();
                });
            }
            else {
                Get_sis_klr_09();
            }
        };
        //---------------------
        function Get_sis_klr_09() {
            if (Layer_sis_klr_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_klr_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_klr_09) {
                    sis_klr_09_value = data_sis_klr_09.features[0].properties.GRAY_INDEX;
                    Get_sis_klr_10();
                });
            }
            else {
                Get_sis_klr_10();
            }
        };
        //---------------------
        function Get_sis_klr_10() {
            if (Layer_sis_klr_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_klr_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_klr_10) {
                    sis_klr_10_value = data_sis_klr_10.features[0].properties.GRAY_INDEX;
                    Get_sis_klr_11();
                });
            }
            else {
                Get_sis_klr_11();
            }
        };
        //---------------------
        function Get_sis_klr_11() {
            if (Layer_sis_klr_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_klr_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_klr_11) {
                    sis_klr_11_value = data_sis_klr_11.features[0].properties.GRAY_INDEX;
                    Get_sis_klr_12();
                });
            }
            else {
                Get_sis_klr_12();
            }
        };
        //---------------------
        function Get_sis_klr_12() {
            if (Layer_sis_klr_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_klr_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_klr_12) {
                    sis_klr_12_value = data_sis_klr_12.features[0].properties.GRAY_INDEX;
                    Get_sic_year();
                });
            }
            else {
                Get_sic_year();
            }
        };
        //---------------------
        function Get_sic_year() {
            if (Layer_sic_year.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sic_year + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sic_year) {
                    sic_year_value = data_sic_year.features[0].properties.GRAY_INDEX;
                    Get_sic_01();
                });
            }
            else {
                Get_sic_01();
            }
        };
        //---------------------
        function Get_sic_01() {
            if (Layer_sic_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sic_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sic_01) {
                    sic_01_value = data_sic_01.features[0].properties.GRAY_INDEX;
                    Get_sic_02();
                });
            }
            else {
                Get_sic_02();
            }
        };
        //---------------------
        function Get_sic_02() {
            if (Layer_sic_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sic_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sic_02) {
                    sic_02_value = data_sic_02.features[0].properties.GRAY_INDEX;
                    Get_sic_03();
                });
            }
            else {
                Get_sic_03();
            }
        };
        //---------------------
        function Get_sic_03() {
            if (Layer_sic_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sic_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sic_03) {
                    sic_03_value = data_sic_03.features[0].properties.GRAY_INDEX;
                    Get_sic_04();
                });
            }
            else {
                Get_sic_04();
            }
        };
        //---------------------
        function Get_sic_04() {
            if (Layer_sic_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sic_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sic_04) {
                    sic_04_value = data_sic_04.features[0].properties.GRAY_INDEX;
                    Get_sic_05();
                });
            }
            else {
                Get_sic_05();
            }
        };
        //---------------------
        function Get_sic_05() {
            if (Layer_sic_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sic_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sic_05) {
                    sic_05_value = data_sic_05.features[0].properties.GRAY_INDEX;
                    Get_sic_06();
                });
            }
            else {
                Get_sic_06();
            }
        };
        //---------------------
        function Get_sic_06() {
            if (Layer_sic_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sic_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sic_06) {
                    sic_06_value = data_sic_06.features[0].properties.GRAY_INDEX;
                    Get_sic_07();
                });
            }
            else {
                Get_sic_07();
            }
        };
        //---------------------
        function Get_sic_07() {
            if (Layer_sic_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sic_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sic_07) {
                    sic_07_value = data_sic_07.features[0].properties.GRAY_INDEX;
                    Get_sic_08();
                });
            }
            else {
                Get_sic_08();
            }
        };
        //---------------------
        function Get_sic_08() {
            if (Layer_sic_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sic_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sic_08) {
                    sic_08_value = data_sic_08.features[0].properties.GRAY_INDEX;
                    Get_sic_09();
                });
            }
            else {
                Get_sic_09();
            }
        };
        //---------------------
        function Get_sic_09() {
            if (Layer_sic_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sic_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sic_09) {
                    sic_09_value = data_sic_09.features[0].properties.GRAY_INDEX;
                    Get_sic_10();
                });
            }
            else {
                Get_sic_10();
            }
        };
        //---------------------
        function Get_sic_10() {
            if (Layer_sic_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sic_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sic_10) {
                    sic_10_value = data_sic_10.features[0].properties.GRAY_INDEX;
                    Get_sic_11();
                });
            }
            else {
                Get_sic_11();
            }
        };
        //---------------------
        function Get_sic_11() {
            if (Layer_sic_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sic_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sic_11) {
                    sic_11_value = data_sic_11.features[0].properties.GRAY_INDEX;
                    Get_sic_12();
                });
            }
            else {
                Get_sic_12();
            }
        };
        //---------------------
        function Get_sic_12() {
            if (Layer_sic_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sic_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sic_12) {
                    sic_12_value = data_sic_12.features[0].properties.GRAY_INDEX;
                    Get_sid_year();
                });
            }
            else {
                Get_sid_year();
            }
        };
        //---------------------
        function Get_sid_year() {
            if (Layer_sid_year.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sid_year + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sid_year) {
                    sid_year_value = data_sid_year.features[0].properties.GRAY_INDEX;
                    Get_sid_01();
                });
            }
            else {
                Get_sid_01();
            }
        };
        //---------------------
        function Get_sid_01() {
            if (Layer_sid_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sid_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sid_01) {
                    sid_01_value = data_sid_01.features[0].properties.GRAY_INDEX;
                    Get_sid_02();
                });
            }
            else {
                Get_sid_02();
            }
        };
        //---------------------
        function Get_sid_02() {
            if (Layer_sid_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sid_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sid_02) {
                    sid_02_value = data_sid_02.features[0].properties.GRAY_INDEX;
                    Get_sid_03();
                });
            }
            else {
                Get_sid_03();
            }
        };
        //---------------------
        function Get_sid_03() {
            if (Layer_sid_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sid_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sid_03) {
                    sid_03_value = data_sid_03.features[0].properties.GRAY_INDEX;
                    Get_sid_04();
                });
            }
            else {
                Get_sid_04();
            }
        };
        //---------------------
        function Get_sid_04() {
            if (Layer_sid_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sid_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sid_04) {
                    sid_04_value = data_sid_04.features[0].properties.GRAY_INDEX;
                    Get_sid_05();
                });
            }
            else {
                Get_sid_05();
            }
        };
        //---------------------
        function Get_sid_05() {
            if (Layer_sid_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sid_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sid_05) {
                    sid_05_value = data_sid_05.features[0].properties.GRAY_INDEX;
                    Get_sid_06();
                });
            }
            else {
                Get_sid_06();
            }
        };
        //---------------------
        function Get_sid_06() {
            if (Layer_sid_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sid_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sid_06) {
                    sid_06_value = data_sid_06.features[0].properties.GRAY_INDEX;
                    Get_sid_07();
                });
            }
            else {
                Get_sid_07();
            }
        };
        //---------------------
        function Get_sid_07() {
            if (Layer_sid_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sid_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sid_07) {
                    sid_07_value = data_sid_07.features[0].properties.GRAY_INDEX;
                    Get_sid_08();
                });
            }
            else {
                Get_sid_08();
            }
        };
        //---------------------
        function Get_sid_08() {
            if (Layer_sid_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sid_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sid_08) {
                    sid_08_value = data_sid_08.features[0].properties.GRAY_INDEX;
                    Get_sid_09();
                });
            }
            else {
                Get_sid_09();
            }
        };
        //---------------------
        function Get_sid_09() {
            if (Layer_sid_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sid_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sid_09) {
                    sid_09_value = data_sid_09.features[0].properties.GRAY_INDEX;
                    Get_sid_10();
                });
            }
            else {
                Get_sid_10();
            }
        };
        //---------------------
        function Get_sid_10() {
            if (Layer_sid_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sid_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sid_10) {
                    sid_10_value = data_sid_10.features[0].properties.GRAY_INDEX;
                    Get_sid_11();
                });
            }
            else {
                Get_sid_11();
            }
        };
        //---------------------
        function Get_sid_11() {
            if (Layer_sid_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sid_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sid_11) {
                    sid_11_value = data_sid_11.features[0].properties.GRAY_INDEX;
                    Get_sid_12();
                });
            }
            else {
                Get_sid_12();
            }
        };
        //---------------------
        function Get_sid_12() {
            if (Layer_sid_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sid_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sid_12) {
                    sid_12_value = data_sid_12.features[0].properties.GRAY_INDEX;
                    Get_sis_year();
                });
            }
            else {
                Get_sis_year();
            }
        };
        //---------------------
        function Get_sis_year() {
            if (Layer_sis_year.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_year + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_year) {
                    sis_year_value = data_sis_year.features[0].properties.GRAY_INDEX;
                    Get_sis_01();
                });
            }
            else {
                Get_sis_01();
            }
        };
        //---------------------
        function Get_sis_01() {
            if (Layer_sis_01.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_01 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_01) {
                    sis_01_value = data_sis_01.features[0].properties.GRAY_INDEX;
                    Get_sis_02();
                });
            }
            else {
                Get_sis_02();
            }
        };
        //---------------------
        function Get_sis_02() {
            if (Layer_sis_02.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_02 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_02) {
                    sis_02_value = data_sis_02.features[0].properties.GRAY_INDEX;
                    Get_sis_03();
                });
            }
            else {
                Get_sis_03();
            }
        };
        //---------------------
        function Get_sis_03() {
            if (Layer_sis_03.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_03 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_03) {
                    sis_03_value = data_sis_03.features[0].properties.GRAY_INDEX;
                    Get_sis_04();
                });
            }
            else {
                Get_sis_04();
            }
        };
        //---------------------
        function Get_sis_04() {
            if (Layer_sis_04.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_04 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_04) {
                    sis_04_value = data_sis_04.features[0].properties.GRAY_INDEX;
                    Get_sis_05();
                });
            }
            else {
                Get_sis_05();
            }
        };
        //---------------------
        function Get_sis_05() {
            if (Layer_sis_05.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_05 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_05) {
                    sis_05_value = data_sis_05.features[0].properties.GRAY_INDEX;
                    Get_sis_06();
                });
            }
            else {
                Get_sis_06();
            }
        };
        //---------------------
        function Get_sis_06() {
            if (Layer_sis_06.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_06 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_06) {
                    sis_06_value = data_sis_06.features[0].properties.GRAY_INDEX;
                    Get_sis_07();
                });
            }
            else {
                Get_sis_07();
            }
        };
        //---------------------
        function Get_sis_07() {
            if (Layer_sis_07.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_07 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_07) {
                    sis_07_value = data_sis_07.features[0].properties.GRAY_INDEX;
                    Get_sis_08();
                });
            }
            else {
                Get_sis_08();
            }
        };
        //---------------------
        function Get_sis_08() {
            if (Layer_sis_08.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_08 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_08) {
                    sis_08_value = data_sis_08.features[0].properties.GRAY_INDEX;
                    Get_sis_09();
                });
            }
            else {
                Get_sis_09();
            }
        };
        //---------------------
        function Get_sis_09() {
            if (Layer_sis_09.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_09 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_09) {
                    sis_09_value = data_sis_09.features[0].properties.GRAY_INDEX;
                    Get_sis_10();
                });
            }
            else {
                Get_sis_10();
            }
        };
        //---------------------
        function Get_sis_10() {
            if (Layer_sis_10.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_10 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_10) {
                    sis_10_value = data_sis_10.features[0].properties.GRAY_INDEX;
                    Get_sis_11();
                });
            }
            else {
                Get_sis_11();
            }
        };
        //---------------------
        function Get_sis_11() {
            if (Layer_sis_11.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_11 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_11) {
                    sis_11_value = data_sis_11.features[0].properties.GRAY_INDEX;
                    Get_sis_12();
                });
            }
            else {
                Get_sis_12();
            }
        };
        //---------------------
        function Get_sis_12() {
            if (Layer_sis_12.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_sis_12 + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_sis_12) {
                    sis_12_value = data_sis_12.features[0].properties.GRAY_INDEX;
                    Get_pamyatnikprirodypol();
                });
            }
            else {
                Get_pamyatnikprirodypol();
            }
        };
        //---------------------
        function Get_pamyatnikprirodypol() {
            if (Layer_pamyatnikprirodypol.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_pamyatnikprirodypol + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_pamyatnikprirodypol) {
                    if (data_pamyatnikprirodypol.features.length > 0) {
                        pamyatnikprirodypol_value = data_pamyatnikprirodypol.features[0].properties.name_rs;
                    }
                    Get_prirparki();
                });
            }
            else {
                Get_prirparki();
            }
        };
        //---------------------
        function Get_prirparki() {
            if (Layer_prirparki.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_prirparki + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_prirparki) {
                    if (data_prirparki.features.length > 0) {
                        prirparki_value = data_prirparki.features[0].properties.name_rs;
                    }
                    Get_rezervaty();
                });
            }
            else {
                Get_rezervaty();
            }
        };
        //---------------------
        function Get_rezervaty() {
            if (Layer_rezervaty.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_rezervaty + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_rezervaty) {
                    if (data_rezervaty.features.length > 0) {
                        rezervaty_value = data_rezervaty.features[0].properties.name_rs;
                    }
                    Get_zakazniky();
                });
            }
            else {
                Get_zakazniky();
            }
        };
        //---------------------
        function Get_zakazniky() {
            if (Layer_zakazniky.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_zakazniky + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_zakazniky) {
                    if (data_zakazniky.features.length > 0) {
                        zakazniky_value = data_zakazniky.features[0].properties.name_rs;
                    }
                    Get_zapovedniki();
                });
            }
            else {
                Get_zapovedniki();
            }
        };
        //---------------------
        function Get_zapovedniki() {
            if (Layer_zapovedniki.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_zapovedniki + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_zapovedniki) {
                    if (data_zapovedniki.features.length > 0) {
                        zapovedniki_value = data_zapovedniki.features[0].properties.name_rs;
                    }
                    Get_arheopamyat();
                });
            }
            else {
                Get_arheopamyat();
            }
        };
        //---------------------
        function Get_arheopamyat() {
            if(Layer_arheopamyat.getVisible()) {
                jQuery.ajax({
                jsonp: false,
                    jsonpCallback: 'getJson',
                        type: 'GET',
                    url: url_arheopamyat + "&format_options=callback:getJson",
                        async: false,
                    dataType: 'jsonp',
                        error: function () {
                    }
                    }).then(function (data_arheopamyat) {
                if (data_arheopamyat.features.length > 0) {
                    arheopamyat_value = data_arheopamyat.features[0].properties.name_rs;
                    }
                Get_kzcover();
                });
                }
            else {
                Get_kzcover();
            }
        };
        //---------------------
        function Get_kzcover() {
            if (Layer_kzcover.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_kzcover + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_kzcover) {
                    if (data_kzcover.features.length > 0) {
                        kzcover_value = data_kzcover.features[0].properties.GRAY_INDEX;
                    }
                    Get_OPTANG();
                });
            }
            else {
                Get_OPTANG();
            }
        };
        //---------------------
        function Get_OPTANG() {
            $.ajax({
                url: '/Maps/GetOPTANG',
                data: JSON.stringify({
                    "Longitude1": parseFloat(longitude.toString().replace(",", ".")),
                    "Latitude1": parseFloat(latitude.toString().replace(",", "."))
                }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data_OPT_ANG) {
                    OPTANG_value = data_OPT_ANG.OPT_ANG;
                    Get_zapovedzony();
                },
                error: function () {
                    alert('error');
                    Get_zapovedzony();
                }
            });
        };
        //---------------------
        // могут быть за границей
        function Get_zapovedzony() {
            if (Layer_zapovedzony.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_zapovedzony + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_zapovedzony) {
                    if (data_zapovedzony.features.length > 0) {
                        zapovedzony_value = data_zapovedzony.features[0].properties.name_rs;
                    }
                    Get_hidroohrzony();
                });
            }
            else {
                Get_hidroohrzony();
            }
        };
        //---------------------
        function Get_hidroohrzony() {
            if (Layer_hidroohrzony.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_hidroohrzony + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_hidroohrzony) {
                    if (data_hidroohrzony.features.length > 0) {
                        hidroohrzony_value = data_hidroohrzony.features[0].properties.name_rs;
                    }
                    Get_meteo_st();
                });
            }
            else {
                Get_meteo_st();
            }
        };
        //---------------------
        function Get_meteo_st() {
            if (Layer_meteo_st.getVisible()) {
                jQuery.ajax({
                    jsonp: false,
                    jsonpCallback: 'getJson',
                    type: 'GET',
                    url: url_meteo_st + "&format_options=callback:getJson",
                    async: false,
                    dataType: 'jsonp',
                    error: function () {
                    }
                }).then(function (data_meteo_st) {
                    if (data_meteo_st.features.length > 0) {
                        meteo_st_name_rs = data_meteo_st.features[0].properties.name_rs;
                        meteo_st_wmo_id = data_meteo_st.features[0].properties.wmo_id;
                    }
                    Get_Last();
                });
            }
            else {
                Get_Last();
            }
        };
        //---------------------
        function Get_Last() {
            //---
            $("#dialog_table").find("tr:gt(0)").remove();
            // КАТО
            if (oblasti_value != '') {
                content = "";
                content += '<tr><td>' + $('#province').html() + '</td>'
                        + '<td>' + oblasti_value + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rayony_value != '') {
                content = "";
                content += '<tr><td>' + $('#district').html() + '</td>'
                        + '<td>' + rayony_value + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            // оптимальные углы наклона
            //if (OPTANG_value[0] != 0) {
            //    var OPTANG_value_s = '';
            //    for (i = 0; i < 12; i++) {
            //        OPTANG_value_s += (i + 1).toString() + ': ' + OPTANG_value[i].toFixed(2).toString() + '; ';
            //    }
            //    content = "";
            //    content += '<tr><td>' + $('#Info_OPTANG').html() + '</td>'
            //            + '<td>' + OPTANG_value_s + '</td></tr>';
            //    $('#dialog_table').append(content);
            //}
            if (OPTANG_value[0] != 0) {
                content = "";
                content += '<tr><td>' + $('#Info_OPTANG').html() + '</td>'
                    + '<td><a href="#" onclick="ShowOPTANGInfoDialog()" style="color: #0000ff;">' + $('#Information').html() + '</a></td>'
                    + '</tr>';
                $('#dialog_OPTANG_01').html(OPTANG_value[0].toFixed(2).toString());
                $('#dialog_OPTANG_02').html(OPTANG_value[1].toFixed(2).toString());
                $('#dialog_OPTANG_03').html(OPTANG_value[2].toFixed(2).toString());
                $('#dialog_OPTANG_04').html(OPTANG_value[3].toFixed(2).toString());
                $('#dialog_OPTANG_05').html(OPTANG_value[4].toFixed(2).toString());
                $('#dialog_OPTANG_06').html(OPTANG_value[5].toFixed(2).toString());
                $('#dialog_OPTANG_07').html(OPTANG_value[6].toFixed(2).toString());
                $('#dialog_OPTANG_08').html(OPTANG_value[7].toFixed(2).toString());
                $('#dialog_OPTANG_09').html(OPTANG_value[8].toFixed(2).toString());
                $('#dialog_OPTANG_10').html(OPTANG_value[9].toFixed(2).toString());
                $('#dialog_OPTANG_11').html(OPTANG_value[10].toFixed(2).toString());
                $('#dialog_OPTANG_12').html(OPTANG_value[11].toFixed(2).toString());
                var OPTANG_sum = OPTANG_value.reduce(function (a, b) { return a + b; });
                var OPTANG_avg = OPTANG_sum / OPTANG_value.length;
                $('#dialog_OPTANG_year').html(OPTANG_avg.toFixed(2).toString());
                $('#dialog_table').append(content);
            }
            // Analize terrain
            if (analizeterrain_value != 255) {
                content = "";
                content += '<tr><td>' + $('#SuitabilityOfTheTerritoryToAccommodateSES').html() + '</td>'
                        + '<td>' + analizeterrain_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            // метеостанция
            if (meteo_st_wmo_id > 0) {
                content = "";
                content += '<tr><td>' + $('#Tree_meteo_st').html() + '</td>'
                    + '<td>' + meteo_st_name_rs + ' (' + meteo_st_wmo_id.toFixed(0) + ')</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            // NASA SSE
            if (avg_dnr_year_value > 0 ||
                            avg_dnr_01_value > 0 ||
                            avg_dnr_02_value > 0 ||
                            avg_dnr_03_value > 0 ||
                            avg_dnr_04_value > 0 ||
                            avg_dnr_05_value > 0 ||
                            avg_dnr_06_value > 0 ||
                            avg_dnr_07_value > 0 ||
                            avg_dnr_08_value > 0 ||
                            avg_dnr_09_value > 0 ||
                            avg_dnr_10_value > 0 ||
                            avg_dnr_11_value > 0 ||
                            avg_dnr_12_value > 0) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_avg_dnr').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (avg_dnr_year_value > 0) {
                content = "";
                content += '<tr><td>' + $('#year').html() + ', ' + $('#Info_avg_dnr_year').html() + '</td>'
                        + '<td>' + avg_dnr_year_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_dnr_01_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + avg_dnr_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_dnr_02_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + avg_dnr_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_dnr_03_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + avg_dnr_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_dnr_04_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + avg_dnr_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_dnr_05_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + avg_dnr_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_dnr_06_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + avg_dnr_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_dnr_07_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + avg_dnr_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_dnr_08_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + avg_dnr_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_dnr_09_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + avg_dnr_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_dnr_10_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + avg_dnr_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_dnr_11_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + avg_dnr_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_dnr_12_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + avg_dnr_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }

            if (swv_dwn_year_value > 0 ||
                swv_dwn_01_value > 0 ||
                swv_dwn_02_value > 0 ||
                swv_dwn_03_value > 0 ||
                swv_dwn_04_value > 0 ||
                swv_dwn_05_value > 0 ||
                swv_dwn_06_value > 0 ||
                swv_dwn_07_value > 0 ||
                swv_dwn_08_value > 0 ||
                swv_dwn_09_value > 0 ||
                swv_dwn_10_value > 0 ||
                swv_dwn_11_value > 0 ||
                swv_dwn_12_value > 0) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_swv_dwn').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (swv_dwn_year_value > 0) {
                content = "";
                content += '<tr><td>' + $('#year').html() + ', ' + $('#Info_avg_dnr_year').html() + '</td>'
                        + '<td>' + swv_dwn_year_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (swv_dwn_01_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + swv_dwn_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (swv_dwn_02_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + swv_dwn_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (swv_dwn_03_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + swv_dwn_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (swv_dwn_04_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + swv_dwn_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (swv_dwn_05_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + swv_dwn_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (swv_dwn_06_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + swv_dwn_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (swv_dwn_07_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + swv_dwn_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (swv_dwn_08_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + swv_dwn_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (swv_dwn_09_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + swv_dwn_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (swv_dwn_10_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + swv_dwn_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (swv_dwn_11_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + swv_dwn_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (swv_dwn_12_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + swv_dwn_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }

            if (exp_dif_year_value > 0 ||
                exp_dif_01_value > 0 ||
                exp_dif_02_value > 0 ||
                exp_dif_03_value > 0 ||
                exp_dif_04_value > 0 ||
                exp_dif_05_value > 0 ||
                exp_dif_06_value > 0 ||
                exp_dif_07_value > 0 ||
                exp_dif_08_value > 0 ||
                exp_dif_09_value > 0 ||
                exp_dif_10_value > 0 ||
                exp_dif_11_value > 0 ||
                exp_dif_12_value > 0) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_exp_dif').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (exp_dif_year_value > 0) {
                content = "";
                content += '<tr><td>' + $('#year').html() + ', ' + $('#Info_avg_dnr_year').html() + '</td>'
                        + '<td>' + exp_dif_year_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (exp_dif_01_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + exp_dif_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (exp_dif_02_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + exp_dif_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (exp_dif_03_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + exp_dif_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (exp_dif_04_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + exp_dif_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (exp_dif_05_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + exp_dif_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (exp_dif_06_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + exp_dif_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (exp_dif_07_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + exp_dif_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (exp_dif_08_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + exp_dif_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (exp_dif_09_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + exp_dif_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (exp_dif_10_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + exp_dif_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (exp_dif_11_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + exp_dif_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (exp_dif_12_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + exp_dif_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }

            if (rettlt0opt_year_value > 0 ||
                            rettlt0opt_01_value > 0 ||
                            rettlt0opt_02_value > 0 ||
                            rettlt0opt_03_value > 0 ||
                            rettlt0opt_04_value > 0 ||
                            rettlt0opt_05_value > 0 ||
                            rettlt0opt_06_value > 0 ||
                            rettlt0opt_07_value > 0 ||
                            rettlt0opt_08_value > 0 ||
                            rettlt0opt_09_value > 0 ||
                            rettlt0opt_10_value > 0 ||
                            rettlt0opt_11_value > 0 ||
                            rettlt0opt_12_value > 0) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_rettlt0opt').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (rettlt0opt_year_value > 0) {
                content = "";
                content += '<tr><td>' + $('#year').html() + ', ' + $('#Info_avg_dnr_year').html() + '</td>'
                        + '<td>' + rettlt0opt_year_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rettlt0opt_01_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + rettlt0opt_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rettlt0opt_02_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + rettlt0opt_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rettlt0opt_03_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + rettlt0opt_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rettlt0opt_04_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + rettlt0opt_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rettlt0opt_05_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + rettlt0opt_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rettlt0opt_06_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + rettlt0opt_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rettlt0opt_07_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + rettlt0opt_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rettlt0opt_08_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + rettlt0opt_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rettlt0opt_09_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + rettlt0opt_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rettlt0opt_10_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + rettlt0opt_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rettlt0opt_11_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + rettlt0opt_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rettlt0opt_12_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + rettlt0opt_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }

            if (clrskyavrg_year_value > 0 ||
                            clrskyavrg_01_value > 0 ||
                            clrskyavrg_02_value > 0 ||
                            clrskyavrg_03_value > 0 ||
                            clrskyavrg_04_value > 0 ||
                            clrskyavrg_05_value > 0 ||
                            clrskyavrg_06_value > 0 ||
                            clrskyavrg_07_value > 0 ||
                            clrskyavrg_08_value > 0 ||
                            clrskyavrg_09_value > 0 ||
                            clrskyavrg_10_value > 0 ||
                            clrskyavrg_11_value > 0 ||
                            clrskyavrg_12_value > 0) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_clrskyavrg').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (clrskyavrg_year_value > 0) {
                content = "";
                content += '<tr><td>' + $('#year').html() + ', ' + $('#Info_avg_dnr_year').html() + '</td>'
                        + '<td>' + clrskyavrg_year_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (clrskyavrg_01_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + clrskyavrg_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (clrskyavrg_02_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + clrskyavrg_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (clrskyavrg_03_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + clrskyavrg_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (clrskyavrg_04_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + clrskyavrg_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (clrskyavrg_05_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + clrskyavrg_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (clrskyavrg_06_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + clrskyavrg_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (clrskyavrg_07_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + clrskyavrg_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (clrskyavrg_08_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + clrskyavrg_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (clrskyavrg_09_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + clrskyavrg_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (clrskyavrg_10_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + clrskyavrg_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (clrskyavrg_11_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + clrskyavrg_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (clrskyavrg_12_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + clrskyavrg_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }

            if (retesh0mim_year_value > 0 ||
                            retesh0mim_01_value > 0 ||
                            retesh0mim_02_value > 0 ||
                            retesh0mim_03_value > 0 ||
                            retesh0mim_04_value > 0 ||
                            retesh0mim_05_value > 0 ||
                            retesh0mim_06_value > 0 ||
                            retesh0mim_07_value > 0 ||
                            retesh0mim_08_value > 0 ||
                            retesh0mim_09_value > 0 ||
                            retesh0mim_10_value > 0 ||
                            retesh0mim_11_value > 0 ||
                            retesh0mim_12_value > 0) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_retesh0mim').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (retesh0mim_year_value > 0) {
                content = "";
                content += '<tr><td>' + $('#year').html() + ', ' + $('#Info_avg_dnr_year').html() + '</td>'
                        + '<td>' + retesh0mim_year_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (retesh0mim_01_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + retesh0mim_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (retesh0mim_02_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + retesh0mim_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (retesh0mim_03_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + retesh0mim_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (retesh0mim_04_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + retesh0mim_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (retesh0mim_05_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + retesh0mim_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (retesh0mim_06_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + retesh0mim_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (retesh0mim_07_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + retesh0mim_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (retesh0mim_08_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + retesh0mim_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (retesh0mim_09_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + retesh0mim_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (retesh0mim_10_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + retesh0mim_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (retesh0mim_11_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + retesh0mim_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (retesh0mim_12_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + retesh0mim_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            // SARAH-E
            if (dni_year_value > 0 ||
                                        dni_01_value > 0 ||
                                        dni_02_value > 0 ||
                                        dni_03_value > 0 ||
                                        dni_04_value > 0 ||
                                        dni_05_value > 0 ||
                                        dni_06_value > 0 ||
                                        dni_07_value > 0 ||
                                        dni_08_value > 0 ||
                                        dni_09_value > 0 ||
                                        dni_10_value > 0 ||
                                        dni_11_value > 0 ||
                                        dni_12_value > 0) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_dni').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (dni_year_value > 0) {
                content = "";
                content += '<tr><td>' + $('#year').html() + ', ' + $('#Info_avg_dnr_year').html() + '</td>'
                        + '<td>' + dni_year_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (dni_01_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + dni_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (dni_02_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + dni_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (dni_03_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + dni_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (dni_04_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + dni_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (dni_05_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + dni_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (dni_06_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + dni_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (dni_07_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + dni_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (dni_08_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + dni_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (dni_09_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + dni_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (dni_10_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + dni_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (dni_11_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + dni_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (dni_12_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + dni_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }

            if (sic_year_value > 0 ||
                            sic_01_value > 0 ||
                            sic_02_value > 0 ||
                            sic_03_value > 0 ||
                            sic_04_value > 0 ||
                            sic_05_value > 0 ||
                            sic_06_value > 0 ||
                            sic_07_value > 0 ||
                            sic_08_value > 0 ||
                            sic_09_value > 0 ||
                            sic_10_value > 0 ||
                            sic_11_value > 0 ||
                            sic_12_value > 0) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_sic').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (sic_year_value > 0) {
                content = "";
                content += '<tr><td>' + $('#year').html() + ', ' + $('#Info_avg_dnr_year').html() + '</td>'
                        + '<td>' + sic_year_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sic_01_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sic_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sic_02_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sic_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sic_03_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sic_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sic_04_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sic_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sic_05_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sic_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sic_06_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sic_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sic_07_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sic_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sic_08_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sic_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sic_09_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sic_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sic_10_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sic_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sic_11_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sic_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sic_12_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sic_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }

            if (sid_year_value > 0 ||
                            sid_01_value > 0 ||
                            sid_02_value > 0 ||
                            sid_03_value > 0 ||
                            sid_04_value > 0 ||
                            sid_05_value > 0 ||
                            sid_06_value > 0 ||
                            sid_07_value > 0 ||
                            sid_08_value > 0 ||
                            sid_09_value > 0 ||
                            sid_10_value > 0 ||
                            sid_11_value > 0 ||
                            sid_12_value > 0) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_sid').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (sid_year_value > 0) {
                content = "";
                content += '<tr><td>' + $('#year').html() + ', ' + $('#Info_avg_dnr_year').html() + '</td>'
                        + '<td>' + sid_year_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sid_01_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sid_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sid_02_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sid_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sid_03_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sid_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sid_04_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sid_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sid_05_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sid_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sid_06_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sid_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sid_07_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sid_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sid_08_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sid_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sid_09_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sid_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sid_10_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sid_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sid_11_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sid_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sid_12_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sid_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }

            if (sis_year_value > 0 ||
                            sis_01_value > 0 ||
                            sis_02_value > 0 ||
                            sis_03_value > 0 ||
                            sis_04_value > 0 ||
                            sis_05_value > 0 ||
                            sis_06_value > 0 ||
                            sis_07_value > 0 ||
                            sis_08_value > 0 ||
                            sis_09_value > 0 ||
                            sis_10_value > 0 ||
                            sis_11_value > 0 ||
                            sis_12_value > 0) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_sis').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (sis_year_value > 0) {
                content = "";
                content += '<tr><td>' + $('#year').html() + ', ' + $('#Info_avg_dnr_year').html() + '</td>'
                        + '<td>' + sis_year_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_01_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_02_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_03_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_04_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_05_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_06_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_07_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_08_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_09_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_10_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_11_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_12_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            // CLARA
            if (sis_klr_year_value > 0 ||
                            sis_klr_01_value > 0 ||
                            sis_klr_02_value > 0 ||
                            sis_klr_03_value > 0 ||
                            sis_klr_04_value > 0 ||
                            sis_klr_05_value > 0 ||
                            sis_klr_06_value > 0 ||
                            sis_klr_07_value > 0 ||
                            sis_klr_08_value > 0 ||
                            sis_klr_09_value > 0 ||
                            sis_klr_10_value > 0 ||
                            sis_klr_11_value > 0 ||
                            sis_klr_12_value > 0) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_sis_klr').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (sis_klr_year_value > 0) {
                content = "";
                content += '<tr><td>' + $('#year').html() + ', ' + $('#Info_avg_dnr_year').html() + '</td>'
                        + '<td>' + sis_klr_year_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_klr_01_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_klr_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_klr_02_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_klr_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_klr_03_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_klr_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_klr_04_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_klr_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_klr_05_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_klr_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_klr_06_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_klr_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_klr_07_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_klr_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_klr_08_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_klr_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_klr_09_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_klr_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_klr_10_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_klr_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_klr_11_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_klr_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (sis_klr_12_value > 0) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_avg_dnr_xx').html() + '</td>'
                        + '<td>' + sis_klr_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            // Климат
            if (rainavgesm_year_value > -1000 ||
                            rainavgesm_01_value > -1000 ||
                            rainavgesm_02_value > -1000 ||
                            rainavgesm_03_value > -1000 ||
                            rainavgesm_04_value > -1000 ||
                            rainavgesm_05_value > -1000 ||
                            rainavgesm_06_value > -1000 ||
                            rainavgesm_07_value > -1000 ||
                            rainavgesm_08_value > -1000 ||
                            rainavgesm_09_value > -1000 ||
                            rainavgesm_10_value > -1000 ||
                            rainavgesm_11_value > -1000 ||
                            rainavgesm_12_value > -1000) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_rainavgesm').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (rainavgesm_year_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#year').html() + ', ' + $('#Info_rainavgesm_year').html() + '</td>'
                        + '<td>' + rainavgesm_year_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rainavgesm_01_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_rainavgesm_xx').html() + '</td>'
                        + '<td>' + rainavgesm_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rainavgesm_02_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_rainavgesm_xx').html() + '</td>'
                        + '<td>' + rainavgesm_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rainavgesm_03_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_rainavgesm_xx').html() + '</td>'
                        + '<td>' + rainavgesm_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rainavgesm_04_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_rainavgesm_xx').html() + '</td>'
                        + '<td>' + rainavgesm_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rainavgesm_05_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_rainavgesm_xx').html() + '</td>'
                        + '<td>' + rainavgesm_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rainavgesm_06_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_rainavgesm_xx').html() + '</td>'
                        + '<td>' + rainavgesm_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rainavgesm_07_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_rainavgesm_xx').html() + '</td>'
                        + '<td>' + rainavgesm_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rainavgesm_08_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_rainavgesm_xx').html() + '</td>'
                        + '<td>' + rainavgesm_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rainavgesm_09_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_rainavgesm_xx').html() + '</td>'
                        + '<td>' + rainavgesm_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rainavgesm_10_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_rainavgesm_xx').html() + '</td>'
                        + '<td>' + rainavgesm_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rainavgesm_11_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_rainavgesm_xx').html() + '</td>'
                        + '<td>' + rainavgesm_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (rainavgesm_12_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_rainavgesm_xx').html() + '</td>'
                        + '<td>' + rainavgesm_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }

            if (t10m_01_value > -1000 ||
                t10m_02_value > -1000 ||
                t10m_03_value > -1000 ||
                t10m_04_value > -1000 ||
                t10m_05_value > -1000 ||
                t10m_06_value > -1000 ||
                t10m_07_value > -1000 ||
                t10m_08_value > -1000 ||
                t10m_09_value > -1000 ||
                t10m_10_value > -1000 ||
                t10m_11_value > -1000 ||
                t10m_12_value > -1000) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_t10m').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_01_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_t10m_xx').html() + '</td>'
                    + '<td>' + t10m_01_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_02_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_t10m_xx').html() + '</td>'
                    + '<td>' + t10m_02_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_03_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_t10m_xx').html() + '</td>'
                    + '<td>' + t10m_03_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_04_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_t10m_xx').html() + '</td>'
                    + '<td>' + t10m_04_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_05_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_t10m_xx').html() + '</td>'
                    + '<td>' + t10m_05_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_06_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_t10m_xx').html() + '</td>'
                    + '<td>' + t10m_06_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_07_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_t10m_xx').html() + '</td>'
                    + '<td>' + t10m_07_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_08_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_t10m_xx').html() + '</td>'
                    + '<td>' + t10m_08_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_09_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_t10m_xx').html() + '</td>'
                    + '<td>' + t10m_09_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_10_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_t10m_xx').html() + '</td>'
                    + '<td>' + t10m_10_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_11_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_t10m_xx').html() + '</td>'
                    + '<td>' + t10m_11_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_12_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_t10m_xx').html() + '</td>'
                    + '<td>' + t10m_12_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }

            if (t10mmax_01_value > -1000 ||
                            t10mmax_02_value > -1000 ||
                            t10mmax_03_value > -1000 ||
                            t10mmax_04_value > -1000 ||
                            t10mmax_05_value > -1000 ||
                            t10mmax_06_value > -1000 ||
                            t10mmax_07_value > -1000 ||
                            t10mmax_08_value > -1000 ||
                            t10mmax_09_value > -1000 ||
                            t10mmax_10_value > -1000 ||
                            t10mmax_11_value > -1000 ||
                            t10mmax_12_value > -1000) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_t10mmax').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (t10mmax_01_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_t10mmax_xx').html() + '</td>'
                        + '<td>' + t10mmax_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10mmax_02_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_t10mmax_xx').html() + '</td>'
                        + '<td>' + t10mmax_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10mmax_03_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_t10mmax_xx').html() + '</td>'
                        + '<td>' + t10mmax_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10mmax_04_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_t10mmax_xx').html() + '</td>'
                        + '<td>' + t10mmax_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10mmax_05_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_t10mmax_xx').html() + '</td>'
                        + '<td>' + t10mmax_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10mmax_06_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_t10mmax_xx').html() + '</td>'
                        + '<td>' + t10mmax_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10mmax_07_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_t10mmax_xx').html() + '</td>'
                        + '<td>' + t10mmax_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10mmax_08_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_t10mmax_xx').html() + '</td>'
                        + '<td>' + t10mmax_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10mmax_09_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_t10mmax_xx').html() + '</td>'
                        + '<td>' + t10mmax_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10mmax_10_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_t10mmax_xx').html() + '</td>'
                        + '<td>' + t10mmax_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10mmax_11_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_t10mmax_xx').html() + '</td>'
                        + '<td>' + t10mmax_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10mmax_12_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_t10mmax_xx').html() + '</td>'
                        + '<td>' + t10mmax_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }

            if (t10m_min_01_value > -1000 ||
                            t10m_min_02_value > -1000 ||
                            t10m_min_03_value > -1000 ||
                            t10m_min_04_value > -1000 ||
                            t10m_min_05_value > -1000 ||
                            t10m_min_06_value > -1000 ||
                            t10m_min_07_value > -1000 ||
                            t10m_min_08_value > -1000 ||
                            t10m_min_09_value > -1000 ||
                            t10m_min_10_value > -1000 ||
                            t10m_min_11_value > -1000 ||
                            t10m_min_12_value > -1000) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_t10m_min').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_min_01_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_t10m_min_xx').html() + '</td>'
                        + '<td>' + t10m_min_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_min_02_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_t10m_min_xx').html() + '</td>'
                        + '<td>' + t10m_min_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_min_03_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_t10m_min_xx').html() + '</td>'
                        + '<td>' + t10m_min_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_min_04_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_t10m_min_xx').html() + '</td>'
                        + '<td>' + t10m_min_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_min_05_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_t10m_min_xx').html() + '</td>'
                        + '<td>' + t10m_min_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_min_06_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_t10m_min_xx').html() + '</td>'
                        + '<td>' + t10m_min_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_min_07_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_t10m_min_xx').html() + '</td>'
                        + '<td>' + t10m_min_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_min_08_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_t10m_min_xx').html() + '</td>'
                        + '<td>' + t10m_min_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_min_09_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_t10m_min_xx').html() + '</td>'
                        + '<td>' + t10m_min_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_min_10_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_t10m_min_xx').html() + '</td>'
                        + '<td>' + t10m_min_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_min_11_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_t10m_min_xx').html() + '</td>'
                        + '<td>' + t10m_min_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (t10m_min_12_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_t10m_min_xx').html() + '</td>'
                        + '<td>' + t10m_min_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }

            if (tskinavg_01_value > -1000 ||
                            tskinavg_02_value > -1000 ||
                            tskinavg_03_value > -1000 ||
                            tskinavg_04_value > -1000 ||
                            tskinavg_05_value > -1000 ||
                            tskinavg_06_value > -1000 ||
                            tskinavg_07_value > -1000 ||
                            tskinavg_08_value > -1000 ||
                            tskinavg_09_value > -1000 ||
                            tskinavg_10_value > -1000 ||
                            tskinavg_11_value > -1000 ||
                            tskinavg_12_value > -1000) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_tskinavg').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (tskinavg_01_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_tskinavg_xx').html() + '</td>'
                        + '<td>' + tskinavg_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (tskinavg_02_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_tskinavg_xx').html() + '</td>'
                        + '<td>' + tskinavg_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (tskinavg_03_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_tskinavg_xx').html() + '</td>'
                        + '<td>' + tskinavg_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (tskinavg_04_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_tskinavg_xx').html() + '</td>'
                        + '<td>' + tskinavg_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (tskinavg_05_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_tskinavg_xx').html() + '</td>'
                        + '<td>' + tskinavg_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (tskinavg_06_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_tskinavg_xx').html() + '</td>'
                        + '<td>' + tskinavg_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (tskinavg_07_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_tskinavg_xx').html() + '</td>'
                        + '<td>' + tskinavg_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (tskinavg_08_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_tskinavg_xx').html() + '</td>'
                        + '<td>' + tskinavg_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (tskinavg_09_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_tskinavg_xx').html() + '</td>'
                        + '<td>' + tskinavg_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (tskinavg_10_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_tskinavg_xx').html() + '</td>'
                        + '<td>' + tskinavg_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (tskinavg_11_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_tskinavg_xx').html() + '</td>'
                        + '<td>' + tskinavg_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (tskinavg_12_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_tskinavg_xx').html() + '</td>'
                        + '<td>' + tskinavg_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }

            if (srfalbavg_01_value > -1000 ||
                            srfalbavg_02_value > -1000 ||
                            srfalbavg_03_value > -1000 ||
                            srfalbavg_04_value > -1000 ||
                            srfalbavg_05_value > -1000 ||
                            srfalbavg_06_value > -1000 ||
                            srfalbavg_07_value > -1000 ||
                            srfalbavg_08_value > -1000 ||
                            srfalbavg_09_value > -1000 ||
                            srfalbavg_10_value > -1000 ||
                            srfalbavg_11_value > -1000 ||
                            srfalbavg_12_value > -1000) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_srfalbavg').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (srfalbavg_01_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + '</td>'
                        + '<td>' + srfalbavg_01_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (srfalbavg_02_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + '</td>'
                        + '<td>' + srfalbavg_02_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (srfalbavg_03_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + '</td>'
                        + '<td>' + srfalbavg_03_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (srfalbavg_04_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + '</td>'
                        + '<td>' + srfalbavg_04_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (srfalbavg_05_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + '</td>'
                        + '<td>' + srfalbavg_05_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (srfalbavg_06_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + '</td>'
                        + '<td>' + srfalbavg_06_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (srfalbavg_07_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + '</td>'
                        + '<td>' + srfalbavg_07_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (srfalbavg_08_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + '</td>'
                        + '<td>' + srfalbavg_08_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (srfalbavg_09_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + '</td>'
                        + '<td>' + srfalbavg_09_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (srfalbavg_10_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + '</td>'
                        + '<td>' + srfalbavg_10_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (srfalbavg_11_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + '</td>'
                        + '<td>' + srfalbavg_11_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            if (srfalbavg_12_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + '</td>'
                        + '<td>' + srfalbavg_12_value.toFixed(2) + '</td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            
            if (avg_kt_22_01_value > -1000 ||
                avg_kt_22_02_value > -1000 ||
                avg_kt_22_03_value > -1000 ||
                avg_kt_22_04_value > -1000 ||
                avg_kt_22_05_value > -1000 ||
                avg_kt_22_06_value > -1000 ||
                avg_kt_22_07_value > -1000 ||
                avg_kt_22_08_value > -1000 ||
                avg_kt_22_09_value > -1000 ||
                avg_kt_22_10_value > -1000 ||
                avg_kt_22_11_value > -1000 ||
                avg_kt_22_12_value > -1000) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_avg_kt_22').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (avg_kt_22_01_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + '</td>'
                    + '<td>' + avg_kt_22_01_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_kt_22_02_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + '</td>'
                    + '<td>' + avg_kt_22_02_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_kt_22_03_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + '</td>'
                    + '<td>' + avg_kt_22_03_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_kt_22_04_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + '</td>'
                    + '<td>' + avg_kt_22_04_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_kt_22_05_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + '</td>'
                    + '<td>' + avg_kt_22_05_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_kt_22_06_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + '</td>'
                    + '<td>' + avg_kt_22_06_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_kt_22_07_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + '</td>'
                    + '<td>' + avg_kt_22_07_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_kt_22_08_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + '</td>'
                    + '<td>' + avg_kt_22_08_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_kt_22_09_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + '</td>'
                    + '<td>' + avg_kt_22_09_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_kt_22_10_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + '</td>'
                    + '<td>' + avg_kt_22_10_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_kt_22_11_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + '</td>'
                    + '<td>' + avg_kt_22_11_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_kt_22_12_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + '</td>'
                    + '<td>' + avg_kt_22_12_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }

            if (avg_nkt_22_01_value > -1000 ||
                avg_nkt_22_02_value > -1000 ||
                avg_nkt_22_03_value > -1000 ||
                avg_nkt_22_04_value > -1000 ||
                avg_nkt_22_05_value > -1000 ||
                avg_nkt_22_06_value > -1000 ||
                avg_nkt_22_07_value > -1000 ||
                avg_nkt_22_08_value > -1000 ||
                avg_nkt_22_09_value > -1000 ||
                avg_nkt_22_10_value > -1000 ||
                avg_nkt_22_11_value > -1000 ||
                avg_nkt_22_12_value > -1000) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_avg_nkt_22').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (avg_nkt_22_01_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + '</td>'
                    + '<td>' + avg_nkt_22_01_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_nkt_22_02_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + '</td>'
                    + '<td>' + avg_nkt_22_02_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_nkt_22_03_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + '</td>'
                    + '<td>' + avg_nkt_22_03_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_nkt_22_04_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + '</td>'
                    + '<td>' + avg_nkt_22_04_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_nkt_22_05_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + '</td>'
                    + '<td>' + avg_nkt_22_05_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_nkt_22_06_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + '</td>'
                    + '<td>' + avg_nkt_22_06_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_nkt_22_07_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + '</td>'
                    + '<td>' + avg_nkt_22_07_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_nkt_22_08_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + '</td>'
                    + '<td>' + avg_nkt_22_08_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_nkt_22_09_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + '</td>'
                    + '<td>' + avg_nkt_22_09_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_nkt_22_10_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + '</td>'
                    + '<td>' + avg_nkt_22_10_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_nkt_22_11_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + '</td>'
                    + '<td>' + avg_nkt_22_11_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (avg_nkt_22_12_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + '</td>'
                    + '<td>' + avg_nkt_22_12_value + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }

            if (day_cld_22_01_value > -1000 ||
                day_cld_22_02_value > -1000 ||
                day_cld_22_03_value > -1000 ||
                day_cld_22_04_value > -1000 ||
                day_cld_22_05_value > -1000 ||
                day_cld_22_06_value > -1000 ||
                day_cld_22_07_value > -1000 ||
                day_cld_22_08_value > -1000 ||
                day_cld_22_09_value > -1000 ||
                day_cld_22_10_value > -1000 ||
                day_cld_22_11_value > -1000 ||
                day_cld_22_12_value > -1000) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_day_cld_22').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (day_cld_22_01_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_day_cld_22_xx').html() + '</td>'
                    + '<td>' + day_cld_22_01_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (day_cld_22_02_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_day_cld_22_xx').html() + '</td>'
                    + '<td>' + day_cld_22_02_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (day_cld_22_03_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_day_cld_22_xx').html() + '</td>'
                    + '<td>' + day_cld_22_03_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (day_cld_22_04_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_day_cld_22_xx').html() + '</td>'
                    + '<td>' + day_cld_22_04_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (day_cld_22_05_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_day_cld_22_xx').html() + '</td>'
                    + '<td>' + day_cld_22_05_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (day_cld_22_06_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_day_cld_22_xx').html() + '</td>'
                    + '<td>' + day_cld_22_06_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (day_cld_22_07_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_day_cld_22_xx').html() + '</td>'
                    + '<td>' + day_cld_22_07_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (day_cld_22_08_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_day_cld_22_xx').html() + '</td>'
                    + '<td>' + day_cld_22_08_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (day_cld_22_09_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_day_cld_22_xx').html() + '</td>'
                    + '<td>' + day_cld_22_09_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (day_cld_22_10_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_day_cld_22_xx').html() + '</td>'
                    + '<td>' + day_cld_22_10_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (day_cld_22_11_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_day_cld_22_xx').html() + '</td>'
                    + '<td>' + day_cld_22_11_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (day_cld_22_12_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_day_cld_22_xx').html() + '</td>'
                    + '<td>' + day_cld_22_12_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }

            if (daylghtav_01_value > -1000 ||
                daylghtav_02_value > -1000 ||
                daylghtav_03_value > -1000 ||
                daylghtav_04_value > -1000 ||
                daylghtav_05_value > -1000 ||
                daylghtav_06_value > -1000 ||
                daylghtav_07_value > -1000 ||
                daylghtav_08_value > -1000 ||
                daylghtav_09_value > -1000 ||
                daylghtav_10_value > -1000 ||
                daylghtav_11_value > -1000 ||
                daylghtav_12_value > -1000) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Tree_daylghtav').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (daylghtav_01_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month01').html() + ', ' + $('#Info_daylghtav_xx').html() + '</td>'
                    + '<td>' + daylghtav_01_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (daylghtav_02_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month02').html() + ', ' + $('#Info_daylghtav_xx').html() + '</td>'
                    + '<td>' + daylghtav_02_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (daylghtav_03_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month03').html() + ', ' + $('#Info_daylghtav_xx').html() + '</td>'
                    + '<td>' + daylghtav_03_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (daylghtav_04_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month04').html() + ', ' + $('#Info_daylghtav_xx').html() + '</td>'
                    + '<td>' + daylghtav_04_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (daylghtav_05_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month05').html() + ', ' + $('#Info_daylghtav_xx').html() + '</td>'
                    + '<td>' + daylghtav_05_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (daylghtav_06_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month06').html() + ', ' + $('#Info_daylghtav_xx').html() + '</td>'
                    + '<td>' + daylghtav_06_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (daylghtav_07_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month07').html() + ', ' + $('#Info_daylghtav_xx').html() + '</td>'
                    + '<td>' + daylghtav_07_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (daylghtav_08_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month08').html() + ', ' + $('#Info_daylghtav_xx').html() + '</td>'
                    + '<td>' + daylghtav_08_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (daylghtav_09_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month09').html() + ', ' + $('#Info_daylghtav_xx').html() + '</td>'
                    + '<td>' + daylghtav_09_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (daylghtav_10_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month10').html() + ', ' + $('#Info_daylghtav_xx').html() + '</td>'
                    + '<td>' + daylghtav_10_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (daylghtav_11_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month11').html() + ', ' + $('#Info_daylghtav_xx').html() + '</td>'
                    + '<td>' + daylghtav_11_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            if (daylghtav_12_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#month12').html() + ', ' + $('#Info_daylghtav_xx').html() + '</td>'
                    + '<td>' + daylghtav_12_value.toFixed(2) + '</td>'
                    + '</tr>';
                $('#dialog_table').append(content);
            }
            // ЛЭП
            if (lep_gid.length > 0) {
                for (i = 0; i < lep_gid.length; i++) {
                    content = "";
                    content += '<tr><td>' + $('#lep_info').html() + '</td>'
                            + '<td><a href="#" onclick="ShowLepInfoDialog(' + lep_gid[i] + ')" style="color: #0000ff;">' + lep_name_rs[i] + '</a></td>'
                            + '</tr>';
                    $('#dialog_table').append(content);
                }
            }
            // СЭС
            var features = [];
            map.forEachFeatureAtPixel(pixel, function (feature, layer) {
                features.push(feature);
            }, null, function (layer) {
                return layer === Layer_spp;
            });
            for (var i = 0, ii = features.length; i < ii; ++i) {
                content = "";
                content += '<tr><td>' + $('#spp').html() + '</td>'
                        + '<td><a href="#" onclick="ShowSPPInfoDialog(' + features[i].get('Id') + ')" style="color: #0000ff;">' + features[i].get('Name') + '</a></td>'
                        + '</tr>';
                $('#dialog_table').append(content);
            }
            // Рельеф
            if (srtm_value > -1000 ||
                aspect_srtm_value > -1000 ||
                slope_srtm_value > -1000) {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#Relief').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (srtm_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#Info_srtm').html() + '</td>'
                        + '<td>' + srtm_value.toFixed(2) + '</td></tr>';
                $('#dialog_table').append(content);
            }
            if (aspect_srtm_value > -1000) {
                content = "";
                var aspect_srtm_value_s = '';
                if (aspect_srtm_value == -1) {
                    aspect_srtm_value_s = $('#Plane').html();
                }
                else if ((aspect_srtm_value > -1 && aspect_srtm_value < 22.5) || (aspect_srtm_value >= 337.5 && aspect_srtm_value <= 360)) {
                    aspect_srtm_value_s = $('#North').html();
                }
                else if (aspect_srtm_value >= 22.5 && aspect_srtm_value < 67.5) {
                    aspect_srtm_value_s = $('#Northeast').html();
                }
                else if (aspect_srtm_value >= 67.5 && aspect_srtm_value < 112.5) {
                    aspect_srtm_value_s = $('#East').html();
                }
                else if (aspect_srtm_value >= 112.5 && aspect_srtm_value < 157.5) {
                    aspect_srtm_value_s = $('#Southeast').html();
                }
                else if (aspect_srtm_value >= 157.5 && aspect_srtm_value < 202.5) {
                    aspect_srtm_value_s = $('#South').html();
                }
                else if (aspect_srtm_value >= 202.5 && aspect_srtm_value < 247.5) {
                    aspect_srtm_value_s = $('#Southwest').html();
                }
                else if (aspect_srtm_value >= 247.5 && aspect_srtm_value < 292.5) {
                    aspect_srtm_value_s = $('#West').html();
                }
                else if (aspect_srtm_value >= 292.5 && aspect_srtm_value < 337.5) {
                    aspect_srtm_value_s = $('#Northwest').html();
                }
                content += '<tr><td>' + $('#Info_aspect_srtm').html() + '</td>'
                        + '<td>' + aspect_srtm_value_s + '</td></tr>';
                $('#dialog_table').append(content);
            }
            if (slope_srtm_value > -1000) {
                content = "";
                content += '<tr><td>' + $('#Info_slope_srtm').html() + '</td>'
                        + '<td>' + slope_srtm_value.toFixed(2) + '</td></tr>';
                $('#dialog_table').append(content);
            }
            // ООПТ
            if (pamyatnikprirodypol_value != '' ||
                prirparki_value != '' ||
                rezervaty_value != '' ||
                zakazniky_value != '' ||
                zapovedniki_value != '' ||
                zapovedzony_value != '') {
                content = "";
                content += '<tr class="table-show-hide-row"><th colspan="2">' + $('#OOPT').html() + ' <span>-</span></th></tr>';
                $('#dialog_table').append(content);
            }
            if (pamyatnikprirodypol_value != '') {
                content = "";
                content += '<tr><td>' + $('#Info_pamyatnikprirodypol').html() + '</td>'
                        + '<td>' + pamyatnikprirodypol_value + '</td></tr>';
                $('#dialog_table').append(content);
            }
            if (prirparki_value != '') {
                content = "";
                content += '<tr><td>' + $('#Info_prirparki').html() + '</td>'
                        + '<td>' + prirparki_value + '</td></tr>';
                $('#dialog_table').append(content);
            }
            if (rezervaty_value != '') {
                content = "";
                content += '<tr><td>' + $('#Info_rezervaty').html() + '</td>'
                        + '<td>' + rezervaty_value + '</td></tr>';
                $('#dialog_table').append(content);
            }
            if (zakazniky_value != '') {
                content = "";
                content += '<tr><td>' + $('#Info_zakazniky').html() + '</td>'
                        + '<td>' + zakazniky_value + '</td></tr>';
                $('#dialog_table').append(content);
            }
            if (zapovedniki_value != '') {
                content = "";
                content += '<tr><td>' + $('#Info_zapovedniki').html() + '</td>'
                        + '<td>' + zapovedniki_value + '</td></tr>';
                $('#dialog_table').append(content);
            }
            if (zapovedzony_value != '') {
                content = "";
                content += '<tr><td>' + $('#Info_zapovedzony').html() + '</td>'
                        + '<td>' + zapovedzony_value + '</td></tr>';
                $('#dialog_table').append(content);
            }
            // памятници археологии
            if (arheopamyat_value != '') {
                content = "";
                content += '<tr><td>' + $('#Info_arheopamyat').html() + '</td>'
                        + '<td>' + arheopamyat_value + '</td></tr>';
                $('#dialog_table').append(content);
            }
            // гидрография
            if (hidroohrzony_value != '') {
                content = "";
                content += '<tr><td>' + $('#Info_hidroohrzony').html() + '</td>'
                        + '<td>' + hidroohrzony_value + '</td></tr>';
                $('#dialog_table').append(content);
            }
            // непригодные и малопригодные земли
            if (kzcover_value != 255) {
                content = "";
                var kzcover_value_s = '';
                if (kzcover_value == 1) {
                    kzcover_value_s = $('#Legend_kzcover_1').html()
                }
                if (kzcover_value == 2) {
                    kzcover_value_s = $('#Legend_kzcover_2').html()
                }
                if (kzcover_value == 3) {
                    kzcover_value_s = $('#Legend_kzcover_3').html()
                }
                if (kzcover_value == 4) {
                    kzcover_value_s = $('#Legend_kzcover_4').html()
                }
                if (kzcover_value == 5) {
                    kzcover_value_s = $('#Legend_kzcover_5').html()
                }
                if (kzcover_value == 6) {
                    kzcover_value_s = $('#Legend_kzcover_6').html()
                }
                if (kzcover_value == 7) {
                    kzcover_value_s = $('#Legend_kzcover_7').html()
                }
                if (kzcover_value == 8) {
                    kzcover_value_s = $('#Legend_kzcover_8').html()
                }
                if (kzcover_value == 9) {
                    kzcover_value_s = $('#Legend_kzcover_9').html()
                }
                content += '<tr><td>' + $('#Info_kzcover').html() + '</td>'
                + '<td>' + kzcover_value_s + '</td></tr>';
                $('#dialog_table').append(content);
            }
            //---

            hdms = ol.coordinate.toStringHDMS(ol.proj.transform(coordinate, 'EPSG:3857', 'EPSG:4326'));
            if (document.getElementById("language").innerHTML == 'kk') {
                hdms = hdms.replace("N", "с. е.");
                hdms = hdms.replace("E", "ш. б.");
            }
            if (document.getElementById("language").innerHTML == 'ru') {
                hdms = hdms.replace("N", "с. ш.");
                hdms = hdms.replace("E", "в. д.");
            }
            document.getElementById("info_coordinates").innerHTML = hdms;
            if (dialog_info_first) {
                $("#dialog_info").dialog({
                    width: 720,
                    height: 300,
                    appendTo: "#fullscreen"
                });
                dialog_info_first = false;
            }
            else {
                $("#dialog_info").dialog({
                    appendTo: "#fullscreen"
                });
            }

            $('.table-show-hide-row').click(function () {
                $(this).find('span').text(function (_, value) {
                    return value == '+' ? '-' : '+';
                });
                $(this).nextUntil('tr.table-show-hide-row').slideToggle(1, function () { });
            });
            $('.table-show-hide-row').css('cursor', 'pointer');

        };
        //---------------------
        Get_oblasti();
        ///////////////////////////////////
    }
    else if (tool == 'calcarea') {
        var coordinate4326 = ol.proj.transform(coordinate, 'EPSG:3857', 'EPSG:4326')
        $("#calcpv_longitude").val(coordinate4326[0]);
        $("#calcpv_latitude").val(coordinate4326[1]);
        if (dialog_calcpv_first) {
            $("#dialog_calcpv").dialog({
                width: 700,
                height: 460,
                appendTo: "#fullscreen"
            });
            dialog_calcpv_first = false;
        }
        else {
            $("#dialog_calcpv").dialog({
                appendTo: "#fullscreen"
            });
        }
    }
    else if (tool == 'calcefficiency') {
        var coordinate4326 = ol.proj.transform(coordinate, 'EPSG:3857', 'EPSG:4326')
        $("#calcefficiency_longitude").val(coordinate4326[0]);
        $("#calcefficiency_latitude").val(coordinate4326[1]);
        if (dialog_calcefficiency_first) {
            $("#dialog_calcefficiency").dialog({
                width: 700,
                height: 490,
                appendTo: "#fullscreen"
            });
            dialog_calcefficiency_first = false;
        }
        else {
            $("#dialog_calcefficiency").dialog({
                appendTo: "#fullscreen"
            });
        }
    }
    else if (tool == 'getmeteodata') {
        var coordinate4326 = ol.proj.transform(coordinate, 'EPSG:3857', 'EPSG:4326')
        $("#get_meteo_data_longitude").val(coordinate4326[0]);
        $("#get_meteo_data_latitude").val(coordinate4326[1]);
        GetMeteoDataPeriodicitiesBySource();
        $("#dialog_get_meteo_data").dialog({
            width: 550,
            height: 250,
            appendTo: "#fullscreen"
        });
    }
    else if (tool == 'createspp') {
        var coordinate4326 = ol.proj.transform(coordinate, 'EPSG:3857', 'EPSG:4326')
        $("#create_spp_longitude").val(coordinate4326[0]);
        $("#create_spp_latitude").val(coordinate4326[1]);
        $("#create_spp_longitude3857").val(coordinate[0]);
        $("#create_spp_latitude3857").val(coordinate[1]);
        $("#dialog_create_spp").dialog({
            width: 720,
            height: 650,
            appendTo: "#fullscreen"
        });
    }
    else if (tool == 'setpoint') {
        var geomx = new ol.geom.Point(
                [coordinate[0], coordinate[1]]
            );
        var is_point = false;
        for (var i = 0; i < Source_CP.getFeatures().length; i++) {
            if (Source_CP.getFeatures()[i].get('Id') == set_point_number) {
                Source_CP.getFeatures()[i].getGeometry().setCoordinates([coordinate[0], coordinate[1]]);
                is_point = true;
                break;
            }
        }
        if (!is_point) {
            var featurex = new ol.Feature({
                Id: set_point_number,
                Name: set_point_number.toString(),
                geometry: geomx
            });
            Source_CP.addFeature(featurex);
        }
        for (var i = 0; i < Source_CP.getFeatures().length; i++) {
            if (Source_CP.getFeatures()[i].get('Id') == 1) {
                $("#compare_points_p1_longitude").html(ol.proj.transform(Source_CP.getFeatures()[i].getGeometry().getCoordinates(), 'EPSG:3857', 'EPSG:4326')[0].toString().replace('.', ','));
                $("#compare_points_p1_latitude").html(ol.proj.transform(Source_CP.getFeatures()[i].getGeometry().getCoordinates(), 'EPSG:3857', 'EPSG:4326')[1].toString().replace('.', ','));
            }
            if (Source_CP.getFeatures()[i].get('Id') == 2) {
                $("#compare_points_p2_longitude").html(ol.proj.transform(Source_CP.getFeatures()[i].getGeometry().getCoordinates(), 'EPSG:3857', 'EPSG:4326')[0].toString().replace('.', ','));
                $("#compare_points_p2_latitude").html(ol.proj.transform(Source_CP.getFeatures()[i].getGeometry().getCoordinates(), 'EPSG:3857', 'EPSG:4326')[1].toString().replace('.', ','));
            }
            if (Source_CP.getFeatures()[i].get('Id') == 3) {
                $("#compare_points_p3_longitude").html(ol.proj.transform(Source_CP.getFeatures()[i].getGeometry().getCoordinates(), 'EPSG:3857', 'EPSG:4326')[0].toString().replace('.', ','));
                $("#compare_points_p3_latitude").html(ol.proj.transform(Source_CP.getFeatures()[i].getGeometry().getCoordinates(), 'EPSG:3857', 'EPSG:4326')[1].toString().replace('.', ','));
            }
        }
    }
});