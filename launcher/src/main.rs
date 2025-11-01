fn main() {
    if let Err(e) = engine_core::run() {
        eprintln!("fatal: {e:?}");
    }
}
