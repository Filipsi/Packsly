namespace Packsly.Core.Common.Model {

    public interface IFactory<T, S> {

        T BuildFrom(S source);

    }

}
