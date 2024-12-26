function UrlsFromBackImage(backgroundImage) {
    let regex = /url\(["']?([^"')]+)["']?\)/g;
    let matches = [...backgroundImage.matchAll(regex)];
    let urls = matches.map(match => match[1]);
    return urls;
}
function VidIDFromUrls(urls) {
    let regex = /\/([^\/]+)small\.jpg$/;
    for (let i = 0; i < urls.length; i++) {
        let match = urls[i].match(regex);
        if (match && match[1]) {
           return match[1];
        }
    }
    return undefined;
}
let shakaPlayerOverlay = document.getElementsByClassName("shaka-player-overlay")[0];
alert(VidIDFromUrls(UrlsFromBackImage(shakaPlayerOverlay.style.backgroundImage)));