<template>
    <div class="ml-auto mr-2">
        <b-nav-item-dropdown
            id="dropdown-grouped"
            variant="link"
            class="dropdown-language border rounded py-50 px-1"
            right
        >
            <template #button-content>
                {{ currentLocale.name }}
            </template>
            <b-dropdown-item
                v-for="localeObj in locales"
                :key="localeObj.locale"
                @click="changeLange(localeObj.locale)"
            >
                <span class="ml-50">{{ localeObj.name }}</span>
            </b-dropdown-item>
        </b-nav-item-dropdown>
    </div>
</template>
<style lang="scss">
.dropdown-language {
    .dropdown-menu {
        min-width: 77px;
    }
}
</style>
<script>
import { BNavItemDropdown, BDropdownItem } from "bootstrap-vue";
import store from '@/store'

export default {
    components: {
        BNavItemDropdown,
        BDropdownItem
    },
    // ! Need to move this computed property to comp function once we get to Vue 3
    computed: {
        currentLocale() {
            return this.locales.find(l => l.locale === this.$i18n.locale);
        },
    },
    setup() {
        const locales = [
            {
                locale: "en",
                name: "EN",
                dir: "ltr"
            },
            {
                locale: "ar",
                name: "AR",
                dir: "rtl"
            }
        ];
        return {
            locales
        };
    },
    methods: {
        changeLange(locale) {
            this.$i18n.locale = locale
            store.commit('appConfig/TOGGLE_RTL')
        }
    },
};
</script>
