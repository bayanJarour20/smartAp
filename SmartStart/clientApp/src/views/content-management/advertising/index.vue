<template>
    <div>
        <b-row class="mt-3">
            <b-col cols="12" md="12" v-show="!advertising.length">
                <b-card>
                    <b-card-title class="text-center m-0"
                        >لايوجد أي إعلانات
                    </b-card-title>
                </b-card>
            </b-col>
        </b-row>
        <b-row>
            <b-col
                cols="12"
                md="6"
                lg="3"
                v-for="(item, i) in activeAdvertising"
                :key="i"
            >
                <component
                    @details="deailsAds"
                    :is="$route.name + '-card'"
                    :item="item"
                ></component>
            </b-col>

            <createAdvertising
                ref="editAdvertisingeDialog"
                title="تعديل الإعلان"
                isEdit
            />
            <b-col cols="12" class="d-flex justify-content-center mb-3">
                <EKPagination :items="advertising" v-model="activeAdvertising" />
            </b-col>
        </b-row>
        
    </div>
</template>
<script>
const AdvertisingCard = () => import("./components/card-advertising.vue");
import { mapActions, mapState, mapGetters } from "vuex";
import createAdvertising from "./components/button-create-advertising.vue";
import EKPagination from "@Ekcore/components/EK-pagination";

export default {
    components: {
        AdvertisingCard,
        createAdvertising,
        EKPagination
    },
    data: () => ({
        activeAdvertising: [],
    }),
    computed: {
        ...mapState({
            advertisingDto: state => state.advertising.advertisingDto
        }),
        ...mapGetters(['advertising'])
    },
    created() {
        this.getAdvertising();
    },
    methods: {
        ...mapActions(["getAdvertising"]),
        deailsAds(item) {
            Object.assign(this.advertisingDto, item);
            this.$refs.editAdvertisingeDialog.openDialog();
        }
    },
    beforeDestroy() {
        this.$store.commit('Reset_Search_Dto')
    },
};
</script>
