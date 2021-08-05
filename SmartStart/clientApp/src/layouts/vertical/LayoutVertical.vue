<template>
    <layout-vertical>
        <template slot="bread-actions">
            <router-view name="bread-actions" v-show="!isLoading" />
        </template>
        <div>
            <router-view/>
        </div>
        <template #navbar="{ toggleVerticalMenuActive }">
            <navbar :toggle-vertical-menu-active="toggleVerticalMenuActive" />
        </template>
<!-- 
        <app-customizer
            v-if="showCustomizer"
            slot="customizer"
        /> -->
    </layout-vertical>
</template>

<script>
import LayoutVertical from "@core/layouts/layout-vertical/LayoutVertical.vue";
// import AppCustomizer from '@core/layouts/components/app-customizer/AppCustomizer.vue'
// import { $themeConfig } from '@themeConfig'
import Navbar from "../components/Navbar.vue";
import { mapGetters } from 'vuex';

export default {
    components: {
        // AppCustomizer,
        LayoutVertical,
        Navbar
    },
    data() {
        return {
            // showCustomizer: $themeConfig.layout.customizer,
        };
    },
    computed: {
        ...mapGetters({
            isLoading: "app/isLoading"
        })
    }
};
</script>
