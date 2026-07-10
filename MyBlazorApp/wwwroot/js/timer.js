window.timerWindows = window.timerWindows || {};

window.abrirJanelaTimer = function (chamadoId, url) {
    const key = "timer_" + chamadoId;
    let win = window.timerWindows[key];

    if (win && !win.closed) {
        win.focus(); // já está aberta, só traz pra frente
    } else {
        win = window.open(url, key, "width=420,height=260,resizable=yes");
        window.timerWindows[key] = win;
    }
};