var gulp = require("gulp"),
    gp_pug = require("gulp-pug");

const node_path = "./node_modules";
const source_path = "./wwwroot_source";
const app_path = source_path + "/app";

var srcPath = {
    templates: [app_path + "/**/*.pug"]
}

gulp.task(
    "templates",
    function() {
        return gulp.src(srcPath.templates)
            .pipe(gp_pug())
            .pipe(gulp.dest(app_path));
    });

gulp.task(
    "watch",
    function() {
        gulp.watch([srcPath.templates], ["templates"]);
    });


gulp.task("default", ["templates", "watch"]);