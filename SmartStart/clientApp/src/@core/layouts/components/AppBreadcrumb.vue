<template>
    <b-row
        v-if="typeof $route.meta == 'function' ? ($route.meta($route).breadcrumb || $route.meta($route).pageTitle) : false"
        class="content-header mb-1"
    >
        <b-col
            class="content-header-left col d-flex align-items-center"
        >
            <h2 class="content-header-title float-left pr-1 mb-0 text-dark">
                {{ $route.meta($route).pageTitle }}
            </h2>
            <div class="breadcrumb-wrapper">
                <b-breadcrumb>
                    <b-breadcrumb-item
                        v-for="item in $route.meta($route).breadcrumb"
                        :key="item.text"
                        :active="item.active"
                        :to="item.to"
                    >
                        <unicon
                            width="20"
                            :fill="item.active ? '#5E5873' : '#A41F02'"
                            class="pr-1"
                            v-if="item.icon"
                            :name="item.icon"
                        />
                            {{ item.text}}
                    </b-breadcrumb-item>
                </b-breadcrumb>
            </div>
        </b-col>
        <!-- Content Right -->
        <b-col cols="6">
            <slot name="bread-actions">
            </slot>
        </b-col>
    </b-row>
</template>
<style lang="scss">
.input-group-text {
    padding: 5px 1rem;
}
</style>
<script>
import {
    BBreadcrumb,
    BBreadcrumbItem,
    BRow,
    BCol,
} from "bootstrap-vue";


export default {
    components: {
        BBreadcrumb,
        BBreadcrumbItem,
        BRow,
        BCol,
    },
};
</script>
