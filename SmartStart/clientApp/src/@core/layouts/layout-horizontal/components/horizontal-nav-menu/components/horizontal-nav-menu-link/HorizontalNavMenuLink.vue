<template>
  <li
    v-if="canViewHorizontalNavMenuLink(item)"
    :class="{
      'active': isActive,
      'disabled': item.disabled
    }"
  >
    <b-link
      v-bind="linkProps"
      class="dropdown-item"
    >
      <feather-icon
        :icon="item.icon || 'CircleIcon'"
        size="24"
      />
      <span class="menu-title">{{  item.title }}</span>
    </b-link>
  </li>
</template>

<script>
import { BLink } from 'bootstrap-vue'
import { useUtils as useAclUtils } from '@core/libs/acl'
import useHorizontalNavMenuLink from './useHorizontalNavMenuLink'
import mixinHorizontalNavMenuLink from './mixinHorizontalNavMenuLink'

export default {
  components: {
    BLink,
  },
  mixins: [mixinHorizontalNavMenuLink],
  props: {
    item: {
      type: Object,
      required: true,
    },
  },
  setup(props) {
    const { isActive, linkProps, updateIsActive } = useHorizontalNavMenuLink(props.item)

    const { canViewHorizontalNavMenuLink } = useAclUtils()

    return {
      isActive,
      linkProps,
      updateIsActive,

      // ACL
      canViewHorizontalNavMenuLink,
    }
  },

}
</script>
