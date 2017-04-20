namespace Packsly.Core.Common.Factory {

    public interface IFactory<T, S> {

        T BuildFrom(S source);

    }

}
