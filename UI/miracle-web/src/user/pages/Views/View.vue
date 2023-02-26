<template>
  <v-app class="div">
    <v-btn @click="isMultiple = !isMultiple">Tek/Çift Görüntü</v-btn>
    <div
      id="viewer"
      @wheel.prevent="wheelEvent($event)"
      @mousedown="mousedown($event)"
      @mouseup="mouseup($event)"
      @mousemove="mousemove($event)"
      draggable="false"
      :style="{
        width: viewerWidth,
        height: viewerHeight,
        overflowX: 'scroll',
        overflowY: 'scroll',
        coordinates,
      }"
      style="text-align: -webkit-center;"
    >
      <v-row :style="{ width }" style="cursor: grab;" class="image">
        <v-col cols="12" md="6">
          <v-img src="/logo.png" />
        </v-col>
        <v-col v-if="isMultiple" cols="12" md="6">
          <v-img src="/message.png" />
        </v-col>
      </v-row>
    </div>
  </v-app>
</template>

<script>
/* eslint-disable*/
export default {
  data() {
    return {
      viewerWidth: "500px",
      viewerHeight: "500px",

      width: "100px",
      height: "100px",
      zoom: 100,
      viewer: "",
      isMultiple: true,
      down: false,

      coordinates: { top: "100px", left: "100px" },
      pos: { top: 0, left: 0, x: 0, y: 0 },
    };
  },
  created() {
    this.viewerWidth = window.innerWidth - 10 + "px";
    this.viewerHeight = window.innerHeight / 1.3 + "px";

    document.documentElement.style.overflow = "hidden";
  },
  methods: {
    //Key pressed
    mousedown(event) {
      if (event.button == 0) {
        this.down = true;
        var viewer = document.getElementById("viewer");

        this.pos = {
          // The current scroll
          left: viewer.scrollLeft,
          top: viewer.scrollTop,
          // Get the current mouse position
          x: event.clientX,
          y: event.clientY,
        };
      }
    },

    mousemove(event) {
      if (this.down == true) {
        this.coordinates.left = event.clientX + "px";
        this.coordinates.top = event.clientY + "px";

        var viewer = document.getElementById("viewer");

        const dx = event.clientX - this.pos.x;
        const dy = event.clientY - this.pos.y;

        // Scroll the element
        viewer.scrollTop = this.pos.top - dy;
        viewer.scrollLeft = this.pos.left - dx;

        window.addEventListener("mouseup", (e) => {
          this.down = false;
          event.preventDefault();
        });
      }
    },

    // Key released
    mouseup(event) {
      this.down = false;
    },

    wheelEvent(event) {
      if (event.deltaY < 0) {
        this.zoom += 50;
      } else {
        this.zoom -= 50;
      }
      if (this.zoom < 50) this.zoom = 50;

      this.width = this.zoom + "px";
      this.height = this.zoom + "px";
    },
  },
};
</script>

<style scoped>
/* Extra small devices (phones, 600px and down) */
@media (min-width: 481px) and (max-width: 767px) {
  .div {
    justify-content: center !important;
    max-height: 638px;
    width: 100%;
  }
}

/* Medium devices (landscape tablets, 768px and 992px) */
@media only screen and (min-width: 768px) and (max-width: 1919px) {
  .div {
    justify-content: center !important;
    max-height: 920px;
    width: 99%;
  }
}

@media only screen and (min-width: 1920px) and (max-width: 1930px) {
  .div {
    justify-content: center !important;
    max-height: 800px;
    width: 100%;
  }
}
</style>
