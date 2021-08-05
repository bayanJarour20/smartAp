<template>
    <router-link
        :to="linkProps.to"
        v-slot="{ href, route, navigate, isActive}"
        v-if="isRoled"
    >
        <li
            v-if="canViewVerticalNavMenuLink(item)"
            class="nav-item aside-nav-items"
            :class="{
            'active': isActive,
            'disabled': item.disabled
            }"
        >
            <a
                :href="href"
                @click="navigate"
                class="d-flex align-items-center"
            >
                <unicon :name="item.icon || 'circle'" width="18" fill="#5E5873" />
                <span class="menu-title text-truncate" style="color: #5E5873">{{ item.title }}</span>
                <b-badge
                    v-if="item.tag"
                    pill
                    :variant="item.tagVariant || 'primary'"
                    class="mr-1 ml-auto"
                >
                    {{ item.tag }}
                </b-badge>
            </a>
        </li>
    </router-link>
</template>
<style lang="scss">
.aside-nav-items.nav-item.active{
  svg path {
    fill: #fff!important;
  }
  span {
    color: #fff!important;
  }
}
</style>
<script>
import { useUtils as useAclUtils } from '@core/libs/acl'
import { BBadge } from 'bootstrap-vue'
import { useUtils as useI18nUtils } from '@core/libs/i18n'
import useVerticalNavMenuLink from './useVerticalNavMenuLink'
import mixinVerticalNavMenuLink from './mixinVerticalNavMenuLink'

export default {
  components: {
    BBadge,
  },
  mixins: [mixinVerticalNavMenuLink],
  props: {
    item: {
      type: Object,
      required: true,
    },
    isRoled: {
        type: Boolean,
        required: true
    }
  },
  setup(props) {
    const { isActive, linkProps, updateIsActive } = useVerticalNavMenuLink(props.item)
    const { t } = useI18nUtils()
    const { canViewVerticalNavMenuLink } = useAclUtils()

    return {
      isActive,
      linkProps,
      updateIsActive,

      // ACL
      canViewVerticalNavMenuLink,

      // i18n
      t,
    }
  },

}
</script>
