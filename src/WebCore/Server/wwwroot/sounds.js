window.playSound = {
    correct: function(volume = 0.1) {
        let audio = new Audio('sounds/correct.wav');
        audio.volume = volume;
        audio.play();
    },
    wrong: function(volume = 0.1) {
        let audio = new Audio('sounds/wrong.wav');
        audio.volume = volume;
        audio.play();
    }
}
