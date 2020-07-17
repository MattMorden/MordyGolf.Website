/// <binding BeforeBuild='dev' ProjectOpened='dev' />
"use strict";
var babel = require("gulp-babel"),
    bundleconfig = require("./bundleconfig.json"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    del = require("del"),
    gulp = require("gulp"),
    sass = require("gulp-sass"),
    merge = require("merge-stream"),
    uglify = require("gulp-uglify");

require("@babel/preset-env");
require("@babel/core");

var regex = {
    cssDev: /^css:dev$/,
    cssProd: /^css:prod$/,
    html: /\.(html|htm)$/,
    jsSite: /^js:site$/,
    scssSite: /^scss:site$/,
    scssPrint: /^scss:print/
};

gulp.task("clean", function () {
    var minifiedFiles = bundleconfig.map(function (bundle) {
        return bundle.outputFileName;
    });

    del(minifiedFiles);

    var expandedFiles = bundleconfig.map(function (bundle) {
        return bundle.localOutputFileName;
    });

    return del(expandedFiles);
});

/* Javascript */
gulp.task("max:js", async function () {
    merge(getBundles(regex.jsSite).map(bundle => {
        return gulp.src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            .pipe(babel({
                "presets": [["@babel/preset-env", { "targets": { "ie": "11" } }]]
            }))
            .pipe(uglify())
            .pipe(gulp.dest('.'));
    }));
});

gulp.task('min:js', async function () {
    merge(getBundles(regex.jsSite).map(bundle => {
        return gulp.src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            .pipe(bable({
                "presets": [["@babel/preset-env", { "targets": { "id": "11" } }]]
            }))
            .pipe(uglify())
            .pipe(gulp.dest('.'));
    }));
});

/* CSS */
gulp.task('max:css', async function () {
    merge(
        getBundles(regex.scssSite).map(bundle => {
            return gulp.src(bundle.inputFiles, { base: '.' })
                .pipe(sass())
                .on("error", function (error) { console.log(error.toString()); this.emit("end"); })
                .pipe(concat(bundle.localOutputFileName))
                .pipe(gulp.dest('.'));
        }),
        getBundles(regex.scssPrint).map(bundle => {
            return gulp.src(bundle.inputFiles, { base: '.' })
                .pipe(sass())
                .on("error", function (error) { console.log(error.toString()); this.emit("end"); })
                .pipe(concat(bundle.localOutputFileName))
                .pipe(gulp.dest('.'));
        })
    );
});

gulp.task('min:css', async function () {
    merge(getBundles(regex.scssSite).map(bundle => {
        return gulp.src(bundle.inputFiles, { base: '.' })
            .pipe(sass())
            .on("error", function (error) { console.log(error.toString()); this.emit("end"); })
            .pipe(conact(bundle.outputFileName))
            .pipe(cssmin())
            .pipe(gulp.des('.'));
    }));
});

/* Combination Tasks */
gulp.task("dev", gulp.series(["clean", "max:css", "max:js"]));
gulp.task("notLocal", gulp.series(["clean", "min:css", "min:js"]));

gulp.task('watch', () => {
    getBundles(regex.scssSite).forEach(
        bundle => gulp.watch("wwwroot/**/!(*.min).scss", gulp.series(["max:css"])));
});

const getBundles = (regexPattern) => {
    return bundleconfig.filter(bundle => {
        return regexPattern.test(bundle.id);
    });
};