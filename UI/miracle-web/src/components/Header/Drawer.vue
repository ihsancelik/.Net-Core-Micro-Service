<template>
  <v-navigation-drawer
    bottom
    color="transparent"
    fixed
    height="auto"
    overlay-color="secondary"
    overlay-opacity=".8"
    temporary
    v-bind="$attrs"
    v-on="$listeners"
  >
    <v-list
      color="white"
      shaped
    >
      <v-list-item
        v-for="item in menus"
        :key="item.name"
        :to="item.to"
        :exact="item.name === 'home'"
        color="primary"
      >
        <v-list-item-content>
          <v-list-item-title v-text='translator(item.name).toUpperCase()' />
        </v-list-item-content>
      </v-list-item>
    </v-list>
  </v-navigation-drawer>
</template>

<script>
  export default {
    name: 'HomeDrawer',

    props: {
      menus: {
        type: Array,
        default: () => ([]),
      },
    },
  }
</script>
